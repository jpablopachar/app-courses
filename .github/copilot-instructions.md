# GitHub Copilot Instructions — app-courses

This is an ASP.NET Core 9 REST API for an online courses platform, built with Clean Architecture and CQRS (MediatR). All new code must follow the conventions documented here.

---

## Architecture

The solution has four projects with strict inward dependency rules:

```
WebApi → Application → Domain
         ↓
Infrastructure / Persistence → Domain
```

- **Domain** — entities, value types, constants. No external dependencies.
- **Application** — MediatR handlers, FluentValidation validators, AutoMapper profiles, interfaces (contracts), DTOs. No infrastructure imports.
- **Infrastructure** — implementations of Application interfaces: `TokenService`, `PhotoService`, `ProfileBuilderService`, `ReportService`.
- **Persistence** — `AppCoursesDbContext`, EF Core entity configurations, migrations, data seeding.
- **WebApi** — controllers, middleware, `Program.cs`, DI extension methods.

---

## CQRS: Commands, Queries & Handlers

Every feature lives in `Application/{Resource}/` as a subfolder. Each feature has its own subfolder.

### File structure for a new feature

```
Application/
  Courses/
    CourseCreate/                          ← command with a request DTO
      CourseCreateCommand.cs               ← IRequest<Result<Guid>>, ICommandBase
      CourseCreateRequest.cs               ← raw input DTO
      CourseCreateCommandHandler.cs
      CourseCreateValidator.cs             ← AbstractValidator<CourseCreateRequest>
      CourseCreateCommandValidator.cs      ← wraps CourseCreateValidator via SetValidator()
    CourseDelete/                          ← command with no request DTO
      CourseDeleteCommand.cs               ← record(Guid? CourseId) : IRequest<Result<Unit>>, ICommandBase
      CourseDeleteCommandHandler.cs
      CourseDeleteCommandValidator.cs      ← AbstractValidator<CourseDeleteCommand> (validates fields directly)
    CourseExcelReport/                     ← query returning a raw stream, not Result<T>
      CourseExcelReportQuery.cs            ← IRequest<MemoryStream>
      CourseExcelReportQueryHandler.cs
    GetCourse/
      GetCourseQuery.cs                    ← IRequest<Result<CourseResponse>>
      GetCourseQueryHandler.cs
    GetCourses/
      GetCoursesQuery.cs                   ← IRequest<Result<PagedList<CourseResponse>>>
      GetCoursesQueryHandler.cs
      GetCoursesRequest.cs                 ← extends PagingParams, adds Title/Description filters
    GetCourseResponse.cs                   ← shared CourseResponse DTO (resource-level, not in a subfolder)
```

### Command skeleton

```csharp
public record CourseCreateCommand(CourseCreateRequest Request) : IRequest<Result<Guid>>;

public class CourseCreateCommandHandler : IRequestHandler<CourseCreateCommand, Result<Guid>>
{
    private readonly AppCoursesDbContext _context;
    private readonly IMapper _mapper;

    public CourseCreateCommandHandler(AppCoursesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CourseCreateCommand request, CancellationToken cancellationToken)
    {
        // ... business logic ...
        await _context.SaveChangesAsync(cancellationToken);
        return Result<T>.Success(entity.Id);
    }
}
```

### Query skeleton

```csharp
public record GetCoursesQuery(PagingParams Params) : IRequest<Result<PagedList<CourseDto>>>;

public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, Result<PagedList<CourseDto>>>
{
    private readonly AppCoursesDbContext _context;
    private readonly IMapper _mapper;

    public GetCoursesQueryHandler(AppCoursesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<CourseDto>>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Courses!.AsNoTracking().AsQueryable();
        // apply filters with ExpressionBuilder...
        var result = await PagedList<CourseDto>.CreateAsync(
            query.ProjectTo<CourseDto>(_mapper.ConfigurationProvider),
            request.Params.PageNumber,
            request.Params.PageSize,
            cancellationToken);
        return Result<PagedList<CourseDto>>.Success(result);
    }
}
```

### Two-layer validation pattern

Commands that wrap a request DTO use **two** validator classes:

```csharp
// 1. Validates the raw request DTO fields
public class CourseCreateValidator : AbstractValidator<CourseCreateRequest>
{
    public CourseCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}

// 2. Wraps the request validator and is discovered by ValidationBehavior
public class CourseCreateCommandValidator : AbstractValidator<CourseCreateCommand>
{
    public CourseCreateCommandValidator()
    {
        RuleFor(x => x.CourseCreateRequest).SetValidator(new CourseCreateValidator());
    }
}
```

Commands with **no request DTO** (e.g., `CourseDeleteCommand(Guid? CourseId)`) use a **single validator** that validates the command fields directly:

```csharp
public class CourseDeleteCommandValidator : AbstractValidator<CourseDeleteCommand>
{
    public CourseDeleteCommandValidator()
    {
        RuleFor(c => c.CourseId).NotNull().WithMessage("CourseId is required.");
    }
}
```

`ValidationBehavior` (MediatR pipeline) runs automatically before every handler. **Never** call validators manually in handlers.

---

## Controllers

All controllers live in `WebApi/Controllers/` and follow this template:

```csharp
/// <summary>
/// Course management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ISender _sender;

    public CoursesController(ISender sender) => _sender = sender;

    /// <summary>Get paginated list of courses.</summary>
    [HttpGet]
    [Authorize(Policy = PolicyMaster.COURSE_READ)]
    [ProducesResponseType(typeof(PagedList<CourseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedList<CourseDto>>> GetCourses(
        [FromQuery] PagingParams pagingParams,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCoursesQuery(pagingParams), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
```

Rules:
- Controllers are **thin** — only call `_sender.Send()` and map the result to an `ActionResult`.
- Always pass `CancellationToken` through to `_sender.Send()`.
- Use explicit HTTP status codes with `[ProducesResponseType]`.
- Apply `[Authorize(Policy = PolicyMaster.XYZ)]` on every protected endpoint.
- Routes use **lowercase kebab-case plural nouns**: `/api/courses`, `/api/instructors`.

---

## Result<T> — expected business failures

Use `Result<T>` from `Application/Core/Result.cs` for expected failures inside handlers:

```csharp
// Return failure
return Result<CourseDto>.Failure("Course not found");

// Return success
return Result<CourseDto>.Success(dto);
```

In the controller:
```csharp
var result = await _sender.Send(query, cancellationToken);
return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
```

**Throw exceptions** only for unexpected infrastructure failures (they are caught by `ExceptionMiddleware`).

---

## DTOs and AutoMapper

- Input DTOs (commands/requests): named `{Verb}Request` — e.g., `CourseCreateRequest`.
- Output DTOs: named `{Noun}Dto` or `{Noun}Response` — e.g., `CourseDto`, `InstructorResponse`.
- **Never** return domain entities from handlers or controllers; always project to a DTO.
- All mappings are defined in `Application/Core/MappingProfile.cs`:

```csharp
CreateMap<Course, CourseDto>();
```

- In handlers use AutoMapper's `ProjectTo<T>` for EF Core queries (avoids loading full entities):

```csharp
query.ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
```

---

## Pagination and filtering

- Queries that return lists accept `PagingParams` (inherits `pageNumber`, `pageSize` max 50, `orderBy`, `orderAsc`).
- Use `PagedList<T>.CreateAsync()` to materialise and return paginated results.
- Build dynamic filters with `ExpressionBuilder`:

```csharp
Expression<Func<Course, bool>> predicate = x => true;
if (!string.IsNullOrEmpty(params.Title))
    predicate = predicate.And(x => x.Title.Contains(params.Title));
query = query.Where(predicate);
```

---

## Authorization

Policies are defined in `Domain/PolicyMaster.cs` and registered in `WebApi/Extensions/PoliciesConfiguration.cs`.  
Each policy checks for a specific claim value in the `POLICIES` claim type.

Available policies (defined in `Domain/PolicyMaster.cs`): `COURSE_READ`, `COURSE_WRITE`, `COURSE_UPDATE`, `COURSE_DELETE`, `INSTRUCTOR_READ`, `INSTRUCTOR_CREATE`, `INSTRUCTOR_UPDATE`, `COMMENT_READ`, `COMMENT_CREATE`, `COMMENT_DELETE`.

When adding a new protected endpoint:
1. Add a constant to `PolicyMaster`.
2. Register it in `PoliciesConfiguration`.
3. Add `[Authorize(Policy = PolicyMaster.NEW_POLICY)]` to the endpoint.
4. Seed the claim to the appropriate role in `DataSeed`.

---

## EF Core patterns

- `DbSet` properties on `AppCoursesDbContext` are **nullable** (e.g., `DbSet<Course>? Courses`). Use the null-forgiving operator (`!`) when calling `Add`, `Remove`, or building queries: `_context.Courses!.AsNoTracking()`.
- Always use `.AsNoTracking()` for read-only queries.
- Prefer `.ProjectTo<TDto>()` over `.Select()` for mapping.
- For write operations, rely on EF Core's implicit transaction via `SaveChangesAsync()`.
- Use explicit transactions with `context.Database.BeginTransactionAsync()` only for multi-step operations that require atomicity.
- Table names use **lowercase snake_case** (configured in `AppCoursesDbContext.OnModelCreating`).
- Never use raw SQL. If unavoidable, use `FromSqlRaw` with `SqlParameter` objects — never string interpolation.

---

## Validation rules

- All validation lives in FluentValidation validators.
- **Never** use `[Required]`, `[MaxLength]`, or any `System.ComponentModel.DataAnnotations` attributes on commands or DTOs.
- Keep validators in the same feature folder as the command/query.

---

## Async patterns

- All controller actions and handlers **must** be `async Task<T>`.
- Always `await` — never `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`.
- Pass `CancellationToken` from the controller action down through `ISender.Send()` and into every `DbContext` call.

---

## Error handling

- `ExceptionMiddleware` (registered first in the pipeline) catches:
  - `ValidationException` → HTTP 400 + `AppException` body with the validation error list.
  - Any other exception → HTTP 500 + `AppException` body.
- The `AppException` shape:
  ```json
  { "statusCode": 400, "message": "...", "details": "..." }
  ```
- Do **not** wrap handler code in try/catch unless you can meaningfully recover.

---

## Dependency Injection

Each layer exposes a single extension method:

| Layer | Method | Registered in |
|---|---|---|
| Application | `AddApplication()` | `Application/DependencyInjection.cs` |
| Persistence | `AddPersistence(config)` | `Persistence/DependencyInjection.cs` |
| Infrastructure | (folded into `AddIdentityService`) | `WebApi/Extensions/IdentityServiceExtensions.cs` |
| WebApi policies | `AddPoliciesServices()` | `WebApi/Extensions/PoliciesConfiguration.cs` |

`Program.cs` only calls these extensions — never raw `services.Add*` there.

When adding a new service:
- Define the **interface** in `Application/Interfaces/`.
- Implement it in `Infrastructure/` or `Persistence/`.
- Register the implementation in the appropriate `DependencyInjection.cs`.

---

## Naming conventions

| Artifact | Pattern | Example |
|---|---|---|
| Command | `{Verb}{Noun}Command` | `CourseCreateCommand` |
| Query | `Get{Noun}Query` | `GetCoursesQuery`, `GetCourseQuery`, `CourseExcelReportQuery` |
| Handler | `{CommandOrQuery}Handler` | `CourseCreateCommandHandler` |
| Request DTO | `{Verb}Request` | `CourseCreateRequest` |
| Response DTO | `{Noun}Dto` / `{Noun}Response` | `CourseDto`, `InstructorResponse` |
| Validator (request) | `{Verb}Validator` | `CourseCreateValidator` |
| Validator (command) | `{CommandOrQuery}Validator` | `CourseCreateCommandValidator` |
| Controller | `{Plural}Controller` | `CoursesController` |
| Extension class | `{Feature}Extensions` | `IdentityServiceExtensions` |
| DB table | lowercase snake_case | `course_instructors` |
| JSON property | camelCase (default) | `firstName` |
| Route | kebab-case plural | `/api/course-categories` |

---

## Roles and claims

- Two roles only: `ADMIN` and `CLIENT` (see `Domain/CustomRoles.cs`).
- Authorization is claim-based via the `POLICIES` claim type (see `Domain/CustomClaims.cs`).
- JWT tokens expire in **7 days** and embed all policy claims for the user's role.

---

## Security checklist for new code

- All DB access through EF Core parameterised queries — no raw SQL string concatenation.
- Never log PII fields: `FullName`, `Email`, `Occupation`.
- Configuration secrets (`TokenKey`, Cloudinary credentials) come from environment variables or User Secrets — never hardcode them.
- Disable `EnableSensitiveDataLogging()` in production builds.
- Restrict the CORS policy to known origins before deploying.

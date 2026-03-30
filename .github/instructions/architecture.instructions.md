---
applyTo: "**/*.cs"
---

# Architecture Instructions — app-courses

## Architectural pattern

This project follows **Clean Architecture** with a strict inward dependency rule. No outer layer may reference an inner layer.

```
WebApi → Application → Domain
         ↓
Infrastructure / Persistence → Domain
```

The solution is divided into five projects:

| Project | Role |
|---|---|
| `Domain` | Entities, value types, constants, roles, claims. Zero external dependencies. |
| `Application` | CQRS handlers, validators, DTOs, interfaces (contracts), AutoMapper profiles, shared utilities. No infrastructure imports. |
| `Infrastructure` | Implementations of `Application` interfaces that integrate with external systems (JWT, Cloudinary). |
| `Persistence` | `AppCoursesDbContext`, EF Core entity configurations, migrations, data seeding. |
| `WebApi` | Controllers, middleware, `Program.cs`, DI extension methods. Depends on `Application` only — never on `Infrastructure` or `Persistence` directly. |

---

## Domain layer

### Entities

All entities inherit from `BaseEntity` which provides a single `Guid Id` property.

```
Domain/
├── BaseEntity.cs            ← abstract base, provides Id
├── Course.cs
├── Instructor.cs
├── Price.cs
├── Qualification.cs
├── Photo.cs
├── CourseInstructor.cs      ← many-to-many junction: Course ↔ Instructor
├── CoursePrice.cs           ← many-to-many junction: Course ↔ Price
├── CustomRoles.cs           ← constants: ADMIN, CLIENT
├── CustomClaims.cs          ← constants: POLICIES claim type
└── PolicyMaster.cs          ← constants: COURSE_READ, COURSE_WRITE, etc.
```

### Rules
- No entity may reference any class outside `Domain`.
- Many-to-many relationships use explicit junction entities (`CourseInstructor`, `CoursePrice`), not EF Core implicit join tables.
- `CustomRoles`, `CustomClaims`, and `PolicyMaster` are `static` classes with `const string` fields. Never use string literals for roles or policies — always reference these constants.

---

## Application layer

### Directory structure

Each resource or feature group has its own subdirectory. Features within a resource each get their own sub-folder.

```
Application/
├── Core/                       ← shared infrastructure for all handlers
│   ├── ICommandBase.cs         ← marker interface for commands (required by ValidationBehavior)
│   ├── Result.cs               ← Result<T>: Success/Failure wrapper
│   ├── PagedList.cs            ← pagination wrapper + CreateAsync factory
│   ├── PagingParams.cs         ← abstract base for query filter/sort params (max page 50)
│   ├── ExpressionBuilder.cs    ← LINQ predicate builder (.And(), .Or())
│   ├── ValidationBehavior.cs   ← MediatR pipeline behavior: runs FluentValidation before every handler
│   ├── ValidationException.cs  ← thrown by ValidationBehavior on failures
│   ├── ValidationError.cs      ← property/message pair in the exception
│   ├── AppException.cs         ← standard error body returned by ExceptionMiddleware
│   └── MappingProfile.cs       ← AutoMapper profile: all entity → DTO mappings
├── Interfaces/                 ← service contracts (implemented in Infrastructure or Persistence)
│   ├── ITokenService.cs
│   ├── IPhotoService.cs
│   ├── IUserAccessor.cs
│   └── IProfileBuilderService.cs
├── Accounts/
│   ├── GetCurrentUser/
│   ├── Login/
│   └── Register/
├── Courses/
│   └── CourseCreate/
├── Instructors/
│   └── GetInstructors/
├── Prices/
│   └── GetPrices/
├── Qualifications/
│   └── GetQualifications/
└── DependencyInjection.cs      ← AddApplication() extension method
```

### CQRS pattern

Every feature is expressed as either a **Command** (mutating) or a **Query** (read-only).

**Command structure** — one folder per operation:
```
Courses/CourseCreate/
├── CourseCreateCommand.cs         ← record : IRequest<Result<T>>, ICommandBase
├── CourseCreateRequest.cs         ← raw input DTO bound from the HTTP body/form
├── CourseCreateCommandHandler.cs  ← IRequestHandler<CourseCreateCommand, Result<T>>
├── CourseCreateValidator.cs       ← AbstractValidator<CourseCreateRequest>
└── CourseCreateCommandValidator.cs← AbstractValidator<CourseCreateCommand>, wraps the above
```

**Query structure** — same folder layout; queries may omit the validator pair if they have no mandatory input.

### Two-layer validation pattern

Every command uses two validator classes:

1. **Request validator** (`CourseCreateValidator`) — validates the raw DTO fields.
2. **Command validator** (`CourseCreateCommandValidator`) — wraps the request validator via `SetValidator()` so `ValidationBehavior` can discover it.

`ValidationBehavior` runs automatically before every handler. Never validate manually inside a handler.

### `ICommandBase` marker interface

`ValidationBehavior<TRequest, TResponse>` has a generic constraint `where TRequest : ICommandBase`. Every command record must implement `ICommandBase`. Queries do not need to implement it.

### `Result<T>`

All handlers return `Result<T>` for expected business outcomes.

```csharp
// In the handler
return Result<Guid>.Failure("Instructor not found");
return Result<Guid>.Success(courseId);

// In the controller
return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
```

Never throw exceptions for expected failures. Throw only for unexpected infrastructure failures — `ExceptionMiddleware` handles them.

### `PagedList<T>` and `PagingParams`

- Queries that return lists accept a `params` object that extends `PagingParams`.
- `PagingParams` provides `PageNumber`, `PageSize` (capped at 50), `OrderBy`, and `OrderAsc`.
- Materialise paginated results with `PagedList<T>.CreateAsync(query, pageNumber, pageSize, cancellationToken)`.

### `ExpressionBuilder`

Build dynamic LINQ predicates with `ExpressionBuilder`:

```csharp
Expression<Func<Course, bool>> predicate = ExpressionBuilder.New<Course>();
if (!string.IsNullOrEmpty(request.Params.Title))
    predicate = predicate.And(x => x.Title!.Contains(request.Params.Title));
var query = _context.Courses!.AsNoTracking().Where(predicate);
```

### `MappingProfile`

All AutoMapper mappings live in `Application/Core/MappingProfile.cs`. Do not create additional profile classes. When adding a new DTO, add `CreateMap<Entity, Dto>()` here.

Use `ProjectTo<TDto>()` in handlers for EF Core queries; use `IMapper.Map<TDto>()` only for in-memory mapping of already-loaded objects.

### Interfaces

Define service contracts in `Application/Interfaces/` only when the implementation lives in `Infrastructure` or `Persistence`. Do not create interfaces for internal Application classes that have a single implementation.

---

## Infrastructure layer

Implements `Application` interfaces that integrate with external systems.

```
Infrastructure/
├── Security/
│   ├── TokenService.cs            ← ITokenService: mints JWT tokens
│   └── UserAccessorService.cs     ← IUserAccessor: reads current user from HttpContext
└── Photos/
    └── PhotoService.cs            ← IPhotoService: Cloudinary upload/delete
```

- No direct reference to `Persistence` or `WebApi`.
- `CloudinarySettings` is bound from configuration via `IOptions<CloudinarySettings>`.
- Infrastructure services are registered in `WebApi/Extensions/IdentityServiceExtensions.cs` alongside Identity.

---

## Persistence layer

```
Persistence/
├── AppCoursesDbContext.cs        ← IdentityDbContext<AppUser>; all DbSets; Fluent API config
├── Models/
│   └── AppUser.cs               ← IdentityUser extension with FullName, Occupation
└── DependencyInjection.cs       ← AddPersistence(config): registers DbContext with SQLite
```

### EF Core conventions

- `DbSet<T>` properties are **nullable** (`DbSet<Course>? Courses`). Always use the null-forgiving operator (`!`) when querying: `_context.Courses!`.
- Table names use **lowercase snake_case**, configured in `OnModelCreating`.
- All read-only queries must use `.AsNoTracking()`.
- Prefer `.ProjectTo<TDto>(_mapper.ConfigurationProvider)` over loading full entities then mapping.
- Use `SaveChangesAsync(cancellationToken)` for implicit transactions. Use `BeginTransactionAsync()` only for multi-step atomic operations.
- Never concatenate raw SQL strings. If raw SQL is unavoidable use `FromSqlRaw` with `SqlParameter` objects.

---

## WebApi layer

### Controllers

- All controllers live in `WebApi/Controllers/` and inherit from `ControllerBase`.
- Inject `ISender` (MediatR) via primary constructor.
- Controllers are **thin**: call `_sender.Send(command, cancellationToken)` and map `Result<T>` to `ActionResult`.
- Always pass `CancellationToken` through to `_sender.Send()`.
- Apply `[Authorize(Policy = PolicyMaster.XYZ)]` on every protected endpoint.
- Use `[ProducesResponseType]` to document expected HTTP status codes.

```csharp
[ApiController]
[Route("api/[controller]")]
public class CoursesController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    [Authorize(Policy = PolicyMaster.COURSE_READ)]
    [ProducesResponseType(typeof(PagedList<CourseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedList<CourseDto>>> GetCourses(
        [FromQuery] CoursePagingParams pagingParams,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCoursesQuery(pagingParams), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
```

### Middleware pipeline order

Middleware is registered in `Program.cs` in this exact order — do not reorder:

1. `ExceptionMiddleware` — catches `ValidationException` → 400, all others → 500.
2. `UseSwaggerDocumentation()` — Swagger UI and OpenAPI spec.
3. `UseCors("corsapp")` — currently open; restrict to known origins before production.
4. `UseAuthentication()` — JWT Bearer validation.
5. `UseAuthorization()` — policy-based access control.
6. `SeedDataAuthentication()` — runs `MigrateAsync()` and seeds initial data if the DB is empty.
7. `MapControllers()`.

### Dependency Injection extensions

Each layer registers its own services through a single extension method. `Program.cs` only calls these — never raw `services.Add*` there:

| Extension method | File | Registers |
|---|---|---|
| `AddApplication()` | `Application/DependencyInjection.cs` | MediatR + `ValidationBehavior`, FluentValidation, AutoMapper |
| `AddPersistence(config)` | `Persistence/DependencyInjection.cs` | `AppCoursesDbContext` (SQLite) |
| `AddIdentityService(config)` | `WebApi/Extensions/IdentityServiceExtensions.cs` | Identity Core, JWT Bearer, `ITokenService`, `IUserAccessor` |
| `AddPoliciesServices()` | `WebApi/Extensions/PoliciesConfiguration.cs` | All authorization policies |
| `AddSwaggerDocumentation()` | `WebApi/Extensions/SwaggerServiceExtensions.cs` | Swagger with Bearer support |

When adding a new service: define the interface in `Application/Interfaces/`, implement it in `Infrastructure/` or `Persistence/`, and register it in the appropriate `DependencyInjection.cs`.

---

## Authorization model

Authorization is **claims-based on top of roles**. The claim type is `CustomClaims.POLICIES`; each claim value is a policy name from `PolicyMaster`.

### How it works end-to-end

1. Client calls `POST /api/account/login`.
2. `LoginCommandHandler` validates credentials via `UserManager`.
3. `TokenService` fetches the user's claims from the DB and mints a JWT (7-day expiry) embedding all `POLICIES` claim values.
4. Protected endpoints declare `[Authorize(Policy = PolicyMaster.COURSE_READ)]`.
5. `PoliciesConfiguration` registers each policy as `RequireAssertion(ctx => ctx.User.HasClaim(c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.XYZ))`.

### Adding a new protected endpoint

1. Add a `const string` to `Domain/PolicyMaster.cs`.
2. Register the policy assertion in `WebApi/Extensions/PoliciesConfiguration.cs`.
3. Decorate the endpoint: `[Authorize(Policy = PolicyMaster.NEW_POLICY)]`.
4. Seed the claim to the appropriate role in `WebApi/Extensions/DataSeed.cs`.

---

## Data flow (happy path)

```
HTTP Request
  → Controller (route matching, auth check)
    → ISender.Send(command/query, cancellationToken)
      → ValidationBehavior (FluentValidation — throws ValidationException on failure)
        → Handler (queries DbContext or calls infrastructure service)
          → Result<T> returned
        → Controller maps Result<T> to ActionResult
HTTP Response
```

---

## What goes where — placement rules

| Artifact | Location |
|---|---|
| Domain entity / value type | `Domain/` |
| Role or policy constant | `Domain/CustomRoles.cs` or `Domain/PolicyMaster.cs` |
| Command or query record | `Application/{Resource}/{Feature}/` |
| Input DTO (request) | Same folder as the command/query |
| Output DTO (response/dto) | Same folder as the query handler |
| MediatR handler | Same folder as the command/query |
| FluentValidation validator | Same folder as the command/query |
| AutoMapper mapping | `Application/Core/MappingProfile.cs` |
| Service interface | `Application/Interfaces/` |
| Service implementation (external) | `Infrastructure/` |
| Service implementation (DB-backed) | `Persistence/` |
| Controller | `WebApi/Controllers/` |
| DI extension | Same project as what it registers |
| Middleware | `WebApi/Middlewares/` |

---

## Key constraints to never violate

- **No circular layer references.** `Application` must never import from `Infrastructure`, `Persistence`, or `WebApi`.
- **No entities in controller responses.** Always project to a DTO before returning.
- **No `DataAnnotations` on commands or DTOs.** Use FluentValidation exclusively.
- **No `.Result` or `.Wait()`.** All async code must use `await`.
- **No raw SQL string concatenation.** All DB access through EF Core parameterised queries.
- **No hard-coded secrets.** `TokenKey` and Cloudinary credentials come from environment variables or User Secrets.
- **No new policy strings as literals.** Always reference `PolicyMaster` constants.

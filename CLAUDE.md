# CLAUDE.md — app-courses

## Project overview

### Core purpose
REST API for an online courses platform. Manages courses, instructors, prices, qualifications, and user accounts. Supports photo uploads via Cloudinary and enforces granular, claims-based authorization.

### Consumer profiles
| Consumer | Notes |
|---|---|
| **Mobile** | Token-based auth (JWT), JSON responses |
| **Web** | SPA-friendly CORS (currently open), Swagger UI for dev |
| **Services** | Internal service-to-service calls follow the same auth flow |

### Performance and scalability targets
- No explicit SLAs defined yet. SQLite is used for local dev/early-stage; swap to a production-grade DB before scaling.
- All I/O operations use `async`/`await`.

### Critical business logic constraints
- Roles are fixed: `ADMIN` and `CLIENT` (defined in `Domain/CustomRoles.cs`).
- Authorization is **claims-based** on top of roles. Every protected endpoint enforces a named policy (e.g., `COURSE_WRITE`). Adding a new action requires a corresponding policy and claim.
- JWT tokens expire in **7 days**. Policies are embedded in the token as `Role` claims, fetched from the DB at login time.
- Courses have a many-to-many relationship with both Instructors and Prices (via junction entities `CourseInstructor` and `CoursePrice`).

---

## Tech stack

| Concern | Choice |
|---|---|
| **Runtime / Framework** | .NET 9 / ASP.NET Core 9 |
| **Language** | C# 13 (nullable reference types enabled globally) |
| **Database** | SQLite (dev) via EF Core 9 code-first |
| **ORM** | Entity Framework Core 9.0.11 |
| **Auth provider** | ASP.NET Core Identity + JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| **CQRS bus** | MediatR 12.1.1 |
| **Mapping** | AutoMapper 13.0.1 |
| **Validation** | FluentValidation 11.3.1 (pipeline behavior) |
| **Seeding** | Bogus (fake data) |
| **Photo storage** | Cloudinary (`CloudinaryDotNet`) |
| **CSV** | CsvHelper |
| **Logging** | ASP.NET Core built-in (`Microsoft.Extensions.Logging`); EF Core query log to Console |
| **Telemetry** | None configured yet |
| **API docs** | Swagger / Swashbuckle + OpenAPI (`AddSwaggerDocumentation`) |

---

## Architecture

### Architectural pattern
**Clean Architecture** — strict inward dependency rule:

```
WebApi → Application → Domain
         ↓
      Infrastructure / Persistence → Domain
```

No outer layer may be referenced by an inner layer.

### Major directories
```
app-courses/
├── Domain/               # Entities, value types, roles, custom claims — zero external deps
├── Application/          # CQRS handlers, DTOs, validators, AutoMapper profiles, interfaces
├── Infrastructure/       # External service implementations (JWT, Cloudinary)
├── Persistence/          # EF Core DbContext, entity configs, migrations, seeding
└── WebApi/               # Controllers, middleware, DI extension methods, Program.cs
```

### Responsibilities of each layer

| Layer | Owns |
|---|---|
| **Domain** | `Course`, `Instructor`, `Price`, `Qualification`, `Photo`, `AppUser`, `CourseInstructor`, `CoursePrice`, `CustomRoles`, `CustomClaims` |
| **Application** | Commands/Queries (MediatR), `ITokenService`, `IPhotoService`, `IUserAccessor`, DTOs, validators, `MappingProfile`, `PagedList<T>`, `Result<T>`, `ValidationBehavior` |
| **Infrastructure** | `TokenService`, `ProfileBuilderService`, `PhotoService`, `CloudinarySettings` |
| **Persistence** | `AppCoursesDbContext`, Fluent API entity configs, `DependencyInjection` (DbContext registration), `DataSeed` |
| **WebApi** | Controllers, `ExceptionMiddleware`, `Program.cs`, extension classes (`IdentityServiceExtensions`, `PoliciesConfiguration`, `SwaggerServiceExtensions`) |

### Middleware / pipeline order
1. `ExceptionMiddleware` — catches `ValidationException` → 400, unhandled → 500 (`AppException`)
2. Swagger / OpenAPI
3. CORS (`corsapp` policy)
4. `UseAuthentication` (JWT)
5. `UseAuthorization` (claims policies)
6. Data seed (`SeedDataAuthentication`) — runs once at startup

### Dependency Injection strategy
- Each layer exposes a single `AddX()` extension on `IServiceCollection`.
  - `AddApplication()` — MediatR, FluentValidation, AutoMapper
  - `AddPersistence(config)` — DbContext (SQLite)
  - `AddIdentityService(config)` — Identity Core, JWT, `ITokenService`, `IUserAccessor`
  - `AddPoliciesServices()` — authorization policies
  - `AddSwaggerDocumentation()` — Swagger with Bearer support
- `Program.cs` only calls these extensions; no raw `services.Add*` there.

### Data flow (happy path)
```
HTTP Request
  → Controller (validates route/auth)
    → ISender.Send(command/query)
      → ValidationBehavior (FluentValidation)
        → Handler (queries DbContext or calls infrastructure service)
          → Result<T> / DTO returned
        → Controller maps to ActionResult
HTTP Response
```

---

## API design standards

### Versioning strategy
No versioning implemented yet. All routes are under `/api/{resource}`. Add versioning via URL segments (`/api/v1/`) when breaking changes are needed.

### Naming conventions

| Concern | Convention |
|---|---|
| Endpoints | Lowercase kebab-case, plural nouns: `/api/courses`, `/api/instructors` |
| JSON properties | **camelCase** (ASP.NET Core default) |
| Commands | `{Action}{Resource}Command` (e.g., `CourseCreateCommand`) |
| Queries | `Get{Resource}Query` (e.g., `GetInstructorsQuery`) |
| Handlers | `{CommandOrQuery}Handler` |
| Validators | `{CommandOrQuery}Validator` |

### Global response format
Successful responses return the DTO directly (no wrapper envelope). Lists use `PagedList<T>` which includes pagination metadata.

### Error schema
`AppException` is the standard error body:
```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "details": "optional stack trace (dev only)"
}
```
Validation errors from FluentValidation are caught by `ExceptionMiddleware` and returned as HTTP 400 with the `AppException` shape plus the list of `ValidationError` objects.

RFC 7807 (`ProblemDetails`) is not currently used; if adopted, update `ExceptionMiddleware` to emit it.

### Pagination, filtering, and sorting
- Use `PagedList<T>` for paginated endpoints. Pass `pageNumber` and `pageSize` as query parameters.
- Dynamic filtering is built with `ExpressionBuilder` (LINQ predicate builder in `Application`).
- Sorting is done in the handler via LINQ `.OrderBy()` / `.OrderByDescending()`.

---

## Coding conventions

### Async / I-O patterns
- All controller actions, handlers, and DB calls must be `async Task<T>`.
- Never use `.Result` or `.Wait()`. Use `await` throughout.
- `CancellationToken` should be threaded from controller actions into MediatR `Send` and `DbContext` calls.

### DTO vs Entity usage
- Entities (`Domain`) are **never** returned from controllers.
- Map entities → DTOs in handlers using AutoMapper (`MappingProfile`).
- Commands carry input data; DTOs carry output data. Keep them separate.

### Validation rules
- All validation lives in FluentValidation validators (`Application/Validators/`).
- Never add `[Required]` or `DataAnnotations` to commands/DTOs — use FluentValidation exclusively.
- `ValidationBehavior` runs automatically via MediatR pipeline before every handler.

### Error handling
- Use `Result<T>` (in `Application/Shared/`) for expected business failures.
- Throw exceptions only for unexpected/infrastructure failures; `ExceptionMiddleware` catches them.
- Do **not** catch exceptions inside handlers unless you can meaningfully recover.

### Typing and interface standards
- Enable nullable reference types (`<Nullable>enable</Nullable>`) and annotate accordingly.
- Define service contracts as interfaces in `Application` (e.g., `ITokenService`, `IPhotoService`). Implement in `Infrastructure` or `Persistence`.

### Logging and tracing
- Inject `ILogger<T>` where needed. Do not use static/global loggers.
- EF Core query logging is enabled in `Persistence/DependencyInjection.cs` (Console sink, `LogLevel.Information`, sensitive data logging **on** — disable in production).
- Add structured log properties with `LoggerMessage` delegates for high-frequency paths.

---

## Security and compliance

### Authentication and authorization flow
1. Client calls `POST /api/account/login` → handler validates credentials via `UserManager`.
2. `TokenService` fetches the user's claims from the DB and mints a JWT (7-day expiry, signed with `TokenKey` from config).
3. Protected endpoints declare `[Authorize(Policy = "COURSE_WRITE")]` (or similar). The JWT is validated, and the claim is checked against the policy.

### Input sanitization and SQLi prevention
- All DB access goes through EF Core parameterized queries. **Never** concatenate raw SQL.
- If raw SQL is needed, use `FromSqlRaw` with `SqlParameter` objects only.
- FluentValidation runs before every handler — reject unexpected characters at the validator level for critical fields.

### CORS and rate limiting
- Current CORS policy (`corsapp`) allows any origin, method, and header. **Restrict to known origins before production.**
- No rate limiting configured yet. Add `AspNetCoreRateLimit` or .NET 8+ built-in `RateLimiter` middleware before going to production.

### Sensitive data handling
- `TokenKey` is stored in configuration (environment variable or secrets manager — never hard-code).
- Cloudinary credentials (`CloudinarySettings`) must come from environment variables / secrets, not `appsettings.json`.
- Disable EF Core sensitive data logging (`EnableSensitiveDataLogging`) in production.
- PII fields (`FullName`, `Email`, `Occupation` on `AppUser`) must not appear in logs.

### Audit logging requirements
- No audit log implemented yet. When added, log `UserId`, action, resource, and timestamp for write operations.

---

## Data and persistence

### Database migration strategy
- Code-first with EF Core Migrations.
- `context.Database.MigrateAsync()` is called at startup (`DataSeed`) to apply pending migrations automatically.
- Create a migration: `dotnet ef migrations add <Name> --project Persistence --startup-project WebApi`
- Never edit a migration file after it has been applied to any shared environment.

### Caching levels and invalidation
- No caching layer yet. Add `IMemoryCache` or Redis via `IDistributedCache` in `Infrastructure` when needed.

### Transaction management
- EF Core's `SaveChangesAsync()` wraps operations in an implicit transaction.
- For multi-step operations requiring atomicity, use `context.Database.BeginTransactionAsync()` explicitly in the handler.

### Query and indexing rules
- Define indexes via Fluent API in `AppCoursesDbContext.OnModelCreating` (or in separate `IEntityTypeConfiguration<T>` classes).
- Avoid loading full entity graphs when only a subset of fields is needed — use `.Select()` projections.
- Use `.AsNoTracking()` for read-only queries.

### Seeding and mock data
- Seeding logic lives in `Persistence/DataSeed.cs`.
- Uses **Bogus** for fake data generation.
- Default seeded accounts: `admin@yopmail.com` (ADMIN) and `jppachar@yopmail.com` (CLIENT). Passwords are set in `DataSeed.cs`.
- Seed data runs only when the DB has no existing records (idempotent guard).

---

## File and logic placement

### Where to create new endpoints / controllers
`WebApi/Controllers/` — inherit from `BaseApiController` (which injects `ISender`). One controller per aggregate root.

### Where to place business logic
- **Application layer only** (handlers). Controllers must be thin — call `ISender.Send()` and return the result.
- Domain rules (invariants) belong in `Domain` entities or domain services.

### When to create abstractions
- Create an interface in `Application` only when the implementation lives in `Infrastructure` or `Persistence` (dependency inversion).
- Do not create interfaces for internal Application services that have a single implementation.

### Naming patterns for files and classes

| Artifact | Pattern | Example |
|---|---|---|
| Command | `{Action}{Resource}Command.cs` | `CourseCreateCommand.cs` |
| Query | `Get{Resource}Query.cs` | `GetInstructorsQuery.cs` |
| Handler | `{CommandOrQuery}Handler.cs` | `CourseCreateCommandHandler.cs` |
| Validator | `{CommandOrQuery}Validator.cs` | `CourseCreateCommandValidator.cs` |
| DTO | `{Resource}Dto.cs` | `CourseDto.cs` |
| Controller | `{Resource}Controller.cs` | `CoursesController.cs` |
| Extension | `{Feature}Extensions.cs` | `IdentityServiceExtensions.cs` |

Commands, queries, and handlers for the same resource live in `Application/{Resource}/` (e.g., `Application/Courses/`).

---

## Specific commands

### Local environment setup
```bash
# Restore dependencies
dotnet restore

# Set secrets (never commit these)
dotnet user-secrets set "TokenKey" "<your-secret>" --project WebApi
dotnet user-secrets set "Cloudinary:CloudName" "<cloud>" --project WebApi
dotnet user-secrets set "Cloudinary:ApiKey" "<key>" --project WebApi
dotnet user-secrets set "Cloudinary:ApiSecret" "<secret>" --project WebApi

# Run API (database is auto-migrated and seeded on startup)
dotnet run --project WebApi
```

### Database migration commands
```bash
# Add a new migration
dotnet ef migrations add <MigrationName> --project Persistence --startup-project WebApi

# Apply pending migrations manually
dotnet ef database update --project Persistence --startup-project WebApi

# Roll back one migration
dotnet ef database update <PreviousMigrationName> --project Persistence --startup-project WebApi

# Remove last unapplied migration
dotnet ef migrations remove --project Persistence --startup-project WebApi
```

### Testing suite commands
```bash
# No test project exists yet.
# When added: dotnet test
```

### Documentation generation
```bash
# Swagger UI is available at runtime:
# http://localhost:<port>/swagger
# OpenAPI JSON: http://localhost:<port>/openapi/v1.json
```

### Build and deployment tasks
```bash
# Build all projects
dotnet build

# Publish for production
dotnet publish WebApi -c Release -o ./publish

# Run published output
dotnet ./publish/WebApi.dll
```

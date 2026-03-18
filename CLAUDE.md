# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run the API (http://localhost:5024)
dotnet run --project WebApi/WebApi.csproj

# EF Core migrations
dotnet ef migrations add <MigrationName> --project Persistence --startup-project WebApi
dotnet ef database update --project Persistence --startup-project WebApi
dotnet ef database drop --project Persistence --startup-project WebApi
```

No test projects exist in the solution at this time.

## Architecture

Five-project layered architecture targeting .NET 9.0:

```
Domain → Persistence → Application → Infrastructure → WebApi
```

- **Domain**: Pure domain entities (Course, Instructor, Qualification, Price, Photo, AppUser). No external dependencies.
- **Persistence**: EF Core + SQLite (`AppCoursesDbContext` extends `IdentityDbContext<AppUser>`). Database seeding via Bogus. DI registered via `AddPersistence()`.
- **Application**: CQRS with MediatR. Contains Commands, Queries, Validators (FluentValidation), AutoMapper profiles, and a `ValidationBehavior` pipeline. Repository interfaces live here.
- **Infrastructure**: JWT token generation (`TokenService`), Cloudinary image uploads, CsvHelper, and repository implementations. DI registered via `AddInfrastructure()`.
- **WebApi**: Controllers, Swagger/OpenAPI, middleware wiring in `Program.cs`.

### CQRS Pattern

All features are structured as Commands (writes) or Queries (reads) under `Application/<Feature>/`:

```
Application/Instructors/GetInstructors/
├── GetInstructorsQuery.cs          # MediatR IRequest<T>
├── GetInstructorsQueryHandler.cs   # IRequestHandler<TRequest, TResponse>
└── InstructorResponse.cs           # Response DTO (often a record)
```

Handlers receive repositories via DI. Commands go through `ValidationBehavior` automatically if a `AbstractValidator<TCommand>` is registered.

### Result Pattern

Operations return `Result<T>` (Success/Failure) rather than throwing exceptions. The custom `ValidationException` carries a `List<ValidationError>` collection.

### Authentication

JWT Bearer authentication. `TokenService` generates tokens embedding user policies as role claims (7-day expiration). `AppUser` extends `IdentityUser` with `FullName` and `Occupation` properties.

### Pagination & Filtering

- `PagedList<T>` for async pagination
- `PagingParams` for page number/size
- `ExpressionBuilder` for dynamic LINQ predicate construction (used in repository queries)

### Code conventions

- All comments and XML documentation are written in **Spanish**
- DTOs use C# `record` types
- Nullable reference types are enabled across all projects
- Implicit usings enabled

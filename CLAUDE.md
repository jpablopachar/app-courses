# CLAUDE.md

This file provides guidance to Claude Code when working with code in this repository.

## Project Overview

.NET 9.0 courses management application built with Clean Architecture principles. Manages courses, instructors, prices, qualifications, and photos with many-to-many relationships.

## Architecture

Clean Architecture with five layers following dependency flow:
```
WebApi → Infrastructure → Application → Persistence → Domain
         ↓                  ↓             ↓
         Application        Domain        Domain
```

### Layers

**Domain**: Core business entities and authorization
- `BaseEntity` with Guid Id for all entities
- Entities: `Course`, `Instructor`, `Price`, `Photo`, `Qualification`
- Join entities: `CourseInstructor`, `CoursePrice`
- Authorization: `PolicyMaster`, `CustomClaims`, `CustomRoles`

**Persistence**: Data access layer
- `AppCoursesDbContext` with entity configurations and seeding
- `AppUser` extending IdentityUser
- SQLite with EF Core 9.0
- Repository pattern implementation

**Application**: Business logic and use cases
- CQRS with MediatR (commands/queries)
- DTOs for API contracts
- Validation with FluentValidation
- AutoMapper for mappings

**Infrastructure**: External services
- File storage, email, external APIs
- Caching, third-party adapters

**WebApi**: API entry point
- REST controllers
- OpenAPI/Swagger
- Authentication/authorization middleware
- Global exception handling

### Domain Relationships

- **Course ↔ Instructor**: via `CourseInstructor`
- **Course ↔ Price**: via `CoursePrice`
- **Course → Qualification**: One-to-many
- **Course → Photo**: One-to-many

## Code Quality Standards

### Clean Code Principles

**Meaningful Names**: Descriptive, intention-revealing identifiers

**Small Functions**: Maximum 20-30 lines, single responsibility

**Single Level of Abstraction**: Don't mix high-level logic with low-level details

**DRY**: Extract common logic, use inheritance/composition appropriately

**Comments**: XML docs for public APIs, explain "why" not "what"

### SOLID Principles (MANDATORY)

**Single Responsibility**: One class, one reason to change. Split classes doing too much.

**Open/Closed**: Open for extension, closed for modification. Use interfaces and abstractions.

**Liskov Substitution**: Derived classes must be substitutable for base classes.

**Interface Segregation**: Many specific interfaces over one general-purpose interface.

**Dependency Inversion**: Depend on abstractions, not concretions. Always use dependency injection.

### Separation of Concerns

**Class Size Limit**: 200 lines maximum. Split if exceeded.

**Controllers**: HTTP concerns only. Business logic goes to Application layer.

**Services**: Split large services into focused, single-purpose services.

**Queries**: Use specification pattern for complex queries.

**Configuration**: Use `IEntityTypeConfiguration<T>` per entity.

**Repositories**: Data access only. Domain logic belongs in domain services/entities.

### Testability Requirements

**Mandatory Practices**:
- Constructor injection for all dependencies
- Interface-based design for services and repositories
- Avoid static methods for business logic
- Prefer pure functions (no side effects)
- Small, focused units
- Never instantiate dependencies with `new` keyword

### Code Organization

**Structure**:
- One class per file
- Organize by feature/entity
- Follow folder structure: `Application/Features/{Entity}/{Commands|Queries}/`

**Naming Conventions**:
- Commands: `{Action}{Entity}Command`
- Queries: `Get{Entity}{Filter}Query`
- Handlers: `{Command|Query}Handler`
- Repositories: `I{Entity}Repository`, `{Entity}Repository`
- Services: `I{Entity}Service`, `{Entity}Service`
- DTOs: `{Entity}Dto`, `{Entity}{Detail}Dto`

### Error Handling

- Domain-specific custom exceptions
- Global exception middleware
- FluentValidation for input validation
- Always log exceptions appropriately

### Async Programming

- Use `async/await` for all I/O operations
- Suffix async methods with "Async"
- Use `async Task` (never `async void` except event handlers)
- Use `ConfigureAwait(false)` in library code

## Commands Reference

**Build**:
```bash
dotnet build                    # Entire solution
dotnet build {ProjectName}      # Specific project
dotnet run --project WebApi     # Run API
```

**Testing**:
```bash
dotnet test                                 # All tests
dotnet test {ProjectName}.Tests             # Specific project
dotnet test /p:CollectCoverage=true         # With coverage
```

**Test Projects Structure** (to be implemented):
- `Domain.Tests`: Domain logic and entity tests
- `Application.Tests`: Business logic, handlers, services
- `Persistence.Tests`: Repository and database integration tests
- `WebApi.Tests`: API endpoint integration tests

## Authorization Model

Policy-based authorization system:

**Defined In**:
- Policies: `Domain/PolicyMaster.cs` (COURSE_*, INSTRUCTOR_*, COMMENT_*)
- Claims: `Domain/CustomClaims.cs` (POLICIES claim type)
- Roles: `Domain/CustomRoles.cs` (ADMIN, CLIENT)

**Rules**:
- ADMIN: Full CRUD on courses, instructors, comments
- CLIENT: Read-only courses/instructors, create comments

**Important**: Use `PolicyMaster` constants, never string literals.

## Technology Stack

- .NET 9.0 with nullable reference types enabled
- Entity Framework Core 9.0
- SQLite (dev), SQL Server/PostgreSQL (production)
- ASP.NET Core Identity
- MediatR for CQRS
- AutoMapper for mappings
- FluentValidation
- Bogus 35.4.0 for data seeding
- OpenAPI/Swagger

## Development Requirements

1. Read existing code before making changes
2. Follow established patterns
3. Write tests for new functionality
4. Keep classes under 200 lines
5. Use interfaces for services and repositories
6. Apply SOLID principles in all decisions
7. Use dependency injection exclusively
8. Document public APIs with XML comments
9. Self-documenting code with meaningful names
10. Extract common logic, avoid duplication

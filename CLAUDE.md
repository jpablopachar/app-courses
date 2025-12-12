# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 9.0 courses management application built with Clean Architecture principles. The solution manages courses, instructors, prices, qualifications, and photos with many-to-many relationships.

## Architecture

The project follows Clean Architecture with clear separation of concerns across five layers:

**Dependency Flow:**
```
WebApi → Infrastructure → Application → Persistence → Domain
         ↓                  ↓             ↓
         Application        Domain        Domain
```

### Layers

- **Domain**: Core business entities and authorization policies. Contains:
  - `BaseEntity` - Base class with Guid Id for all entities
  - Domain entities: `Course`, `Instructor`, `Price`, `Photo`, `Qualification`
  - Join entities for many-to-many relationships: `CourseInstructor`, `CoursePrice`
  - Authorization infrastructure: `PolicyMaster` (defines policies like COURSE_READ, COURSE_WRITE), `CustomClaims`, `CustomRoles`

- **Persistence**: Data access layer (currently empty, references Domain)

- **Application**: Business logic and use cases (currently empty, references Domain and Persistence)

- **Infrastructure**: External concerns and services (currently empty, references Application)

- **WebApi**: ASP.NET Core Web API entry point with controllers and OpenAPI configuration

### Domain Model Relationships

The domain uses many-to-many relationships with explicit join entities:

- **Course ↔ Instructor**: Through `CourseInstructor` join entity
- **Course ↔ Price**: Through `CoursePrice` join entity
- **Course → Qualification**: One-to-many (qualifications belong to courses)
- **Course → Photo**: One-to-many (photos belong to courses)

## Building and Running

```bash
# Build the solution
dotnet build

# Run the Web API
dotnet run --project WebApi

# Build specific project
dotnet build Domain
dotnet build Application
dotnet build Persistence
dotnet build Infrastructure
dotnet build WebApi
```

## Testing

```bash
# Run all tests (when test projects are added)
dotnet test

# Run tests for specific project
dotnet test ProjectName.Tests
```

## Authorization Model

The application uses a policy-based authorization system:

- **Policies** are defined in `Domain/PolicyMaster.cs` (COURSE_READ, COURSE_WRITE, COURSE_UPDATE, COURSE_DELETE, INSTRUCTOR_*, COMMENT_*)
- **Custom Claims** are defined in `Domain/CustomClaims.cs` (POLICIES claim type)
- **Custom Roles** are defined in `Domain/CustomRoles.cs`

When implementing authorization features, use the constants from `PolicyMaster` rather than string literals.

## Development Notes

- Target framework: .NET 9.0
- Nullable reference types are enabled across all projects
- All domain entities inherit from `BaseEntity` which provides a `Guid Id` property
- The codebase uses XML documentation comments extensively for public APIs

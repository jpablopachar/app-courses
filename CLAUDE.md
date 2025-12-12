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

- **Persistence**: Data access layer with:
  - `AppCoursesDbContext` - EF Core DbContext with entity configurations and data seeding
  - `AppUser` - Extended IdentityUser model
  - `DependencyInjection` - Service registration for persistence layer
  - Uses SQLite with EF Core 9.0
  - Implements repository pattern for data access

- **Application**: Business logic and use cases layer (to be implemented):
  - CQRS pattern with MediatR for commands and queries
  - DTOs (Data Transfer Objects) for API contracts
  - Business logic validation and rules
  - Service interfaces and implementations
  - AutoMapper for entity-to-DTO mappings

- **Infrastructure**: External concerns and services (to be implemented):
  - File storage services (photo uploads)
  - Email services
  - External API integrations
  - Caching implementations
  - Third-party service adapters

- **WebApi**: ASP.NET Core Web API entry point:
  - Controllers for REST API endpoints
  - OpenAPI/Swagger configuration
  - Authentication and authorization middleware
  - Global exception handling
  - API versioning

### Domain Model Relationships

The domain uses many-to-many relationships with explicit join entities:

- **Course ↔ Instructor**: Through `CourseInstructor` join entity
- **Course ↔ Price**: Through `CoursePrice` join entity
- **Course → Qualification**: One-to-many (qualifications belong to courses)
- **Course → Photo**: One-to-many (photos belong to courses)

## Code Quality Principles

**This project enforces strict code quality standards. ALL code must adhere to these principles:**

### 1. Clean Code Standards

- **Meaningful Names**: Use descriptive, intention-revealing names for classes, methods, and variables
  - ✅ `GetCoursesByInstructorIdAsync(Guid instructorId)`
  - ❌ `GetData(Guid id)` or `Process()`

- **Small Functions**: Keep methods focused on a single responsibility
  - Maximum 20-30 lines per method
  - If a method is too long, extract helper methods
  - Each method should do ONE thing well

- **Single Level of Abstraction**: Keep operations at the same level within a method
  - Don't mix high-level business logic with low-level implementation details

- **DRY (Don't Repeat Yourself)**: Avoid code duplication
  - Extract common logic into reusable methods or services
  - Use inheritance or composition when appropriate

- **Comments**: Code should be self-explanatory
  - Use XML documentation for public APIs
  - Add comments only to explain "why", not "what"
  - Avoid obvious comments that repeat what the code does

### 2. SOLID Principles (MANDATORY)

#### **S - Single Responsibility Principle**
- Each class should have ONE reason to change
- If a class is doing too much, split it into multiple classes
- Examples:
  - ✅ `CourseRepository` - handles only data access
  - ✅ `CourseValidator` - handles only validation logic
  - ❌ `CourseService` that handles validation, data access, and email notifications

#### **O - Open/Closed Principle**
- Classes should be open for extension but closed for modification
- Use interfaces and abstract classes for extensibility
- Prefer composition over modification of existing code

#### **L - Liskov Substitution Principle**
- Derived classes must be substitutable for their base classes
- Don't break parent class contracts in derived classes

#### **I - Interface Segregation Principle**
- Many specific interfaces are better than one general-purpose interface
- Clients shouldn't depend on methods they don't use
- Examples:
  - ✅ `IReadRepository<T>` and `IWriteRepository<T>`
  - ❌ Single `IRepository<T>` with 20 methods when you only need 3

#### **D - Dependency Inversion Principle**
- Depend on abstractions (interfaces), not concretions
- High-level modules shouldn't depend on low-level modules
- Use dependency injection for all external dependencies
- Example:
  ```csharp
  // ✅ GOOD - Depends on abstraction
  public class CourseService
  {
      private readonly ICourseRepository _repository;
      public CourseService(ICourseRepository repository) => _repository = repository;
  }

  // ❌ BAD - Depends on concrete implementation
  public class CourseService
  {
      private readonly CourseRepository _repository = new();
  }
  ```

### 3. Separation of Concerns

**If a class exceeds 200 lines or has multiple responsibilities, SPLIT IT:**

- **Large Controllers**: Extract business logic to Application layer services
  - Controllers should only handle HTTP concerns (routing, model binding, responses)
  - Business logic belongs in Application layer handlers/services

- **Large Services**: Split into multiple focused services
  - Example: `CourseManagementService` → `CourseCreationService`, `CourseUpdateService`, `CourseQueryService`

- **Complex Queries**: Use specification pattern or separate query classes
  - Extract complex LINQ queries into reusable specifications

- **Large Configuration Classes**: Split DbContext configurations by entity
  - Use `IEntityTypeConfiguration<T>` for each entity

- **Business Logic in Repositories**: Extract to domain services
  - Repositories should only handle data access
  - Domain logic belongs in domain services or entities themselves

### 4. Testability Requirements

**All code MUST be written with testing in mind:**

- **Dependency Injection**: Use constructor injection for all dependencies
  - Makes mocking and testing easier
  - Explicit dependencies in constructor signature

- **Avoid Static Dependencies**: Don't use static methods for business logic
  - Static methods are hard to mock
  - Use interfaces and instance methods instead

- **Pure Functions When Possible**: Functions without side effects are easier to test
  - Same input always produces same output
  - No hidden dependencies or state modifications

- **Small, Focused Units**: Each class/method should test one thing
  - Easier to write focused unit tests
  - Better code coverage

- **Interface-Based Design**: Use interfaces for all services and repositories
  - Enables mocking with frameworks like Moq or NSubstitute

- **Avoid New Keyword**: Don't instantiate dependencies directly
  - Use dependency injection instead
  - Example:
    ```csharp
    // ✅ GOOD - Testable
    public class CourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IEmailService _emailService;

        public CourseService(ICourseRepository repo, IEmailService emailService)
        {
            _repo = repo;
            _emailService = emailService;
        }
    }

    // ❌ BAD - Hard to test
    public class CourseService
    {
        private readonly CourseRepository _repo = new CourseRepository();

        public void CreateCourse()
        {
            var emailService = new EmailService(); // Can't mock this!
        }
    }
    ```

### 5. Code Organization Guidelines

- **One Class Per File**: Each class should be in its own file
- **Logical Folder Structure**: Organize by feature or entity
  - Application/Features/Courses/Commands/
  - Application/Features/Courses/Queries/
  - Application/Features/Instructors/Commands/
- **Naming Conventions**:
  - Commands: `CreateCourseCommand`, `UpdateCourseCommand`
  - Queries: `GetCourseByIdQuery`, `GetCoursesListQuery`
  - Handlers: `CreateCourseCommandHandler`, `GetCourseByIdQueryHandler`
  - Repositories: `ICourseRepository`, `CourseRepository`
  - Services: `ICourseService`, `CourseService`
  - DTOs: `CourseDto`, `CourseDetailDto`, `CourseCreateDto`

### 6. Error Handling

- **Use Custom Exceptions**: Create domain-specific exceptions
  - Example: `CourseNotFoundException`, `InvalidCourseDataException`
- **Global Exception Middleware**: Handle exceptions centrally
- **Validation**: Use FluentValidation for input validation
- **Don't Swallow Exceptions**: Always log and handle appropriately

### 7. Asynchronous Programming

- **Use async/await**: For all I/O operations (database, file system, HTTP)
- **Suffix with Async**: All async methods should end with "Async"
  - `GetCourseByIdAsync()`, `CreateCourseAsync()`
- **Avoid async void**: Use `async Task` instead (except for event handlers)
- **ConfigureAwait**: Use `ConfigureAwait(false)` in library code

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

# Run with coverage
dotnet test /p:CollectCoverage=true
```

### Test Project Structure (to be implemented)
- **Domain.Tests**: Domain logic and entity tests
- **Application.Tests**: Business logic, handlers, and service tests
- **Persistence.Tests**: Repository and database tests (integration tests)
- **WebApi.Tests**: API endpoint tests (integration tests)

## Authorization Model

The application uses a policy-based authorization system:

- **Policies** are defined in `Domain/PolicyMaster.cs` (COURSE_READ, COURSE_WRITE, COURSE_UPDATE, COURSE_DELETE, INSTRUCTOR_*, COMMENT_*)
- **Custom Claims** are defined in `Domain/CustomClaims.cs` (POLICIES claim type)
- **Custom Roles** are defined in `Domain/CustomRoles.cs` (ADMIN, CLIENT)

**Authorization Rules:**
- Admin role: Full CRUD access to courses, instructors, and comments
- Client role: Read-only access to courses and instructors, can create comments

When implementing authorization features, use the constants from `PolicyMaster` rather than string literals.

## Development Notes

- **Target framework**: .NET 9.0
- **Nullable reference types**: Enabled across all projects
- **Implicit usings**: Enabled globally
- **Entity base class**: All domain entities inherit from `BaseEntity` with `Guid Id`
- **XML documentation**: Required for all public APIs
- **Database**: SQLite with Entity Framework Core 9.0
- **Data seeding**: Uses Bogus library for generating realistic test data
- **Identity**: ASP.NET Core Identity for user authentication

## Technology Stack

- **Web Framework**: ASP.NET Core 9.0
- **ORM**: Entity Framework Core 9.0
- **Database**: SQLite (development), can be changed to SQL Server or PostgreSQL for production
- **Authentication**: ASP.NET Core Identity
- **API Documentation**: OpenAPI/Swagger
- **Fake Data**: Bogus 35.4.0

## Important Reminders

1. **Always read existing code** before making changes
2. **Follow the existing patterns** in the codebase
3. **Write tests** for new functionality
4. **Keep classes small** - split if over 200 lines
5. **Use interfaces** for all services and repositories
6. **Apply SOLID principles** in every design decision
7. **Make code testable** - use dependency injection
8. **Document public APIs** with XML comments
9. **Use meaningful names** - code should be self-documenting
10. **Don't repeat yourself** - extract common logic

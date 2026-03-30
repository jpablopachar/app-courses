---
applyTo: "**/*.cs"
---

# Code Documentation Instructions — app-courses

## Language and format

- All XML documentation comments must be written in **Spanish**.
- Use C# triple-slash `///` comments exclusively. Never use block comments (`/** */`) for documentation.
- Every public type (class, record, interface, enum) and every public member (method, property, constructor) must have at minimum a `<summary>` tag.
- Internal and private members do not require documentation unless the logic is non-obvious.

---

## Required XML tags by member type

### Classes, records, and interfaces

```csharp
/// <summary>
/// Descripción breve de la responsabilidad de la clase.
/// </summary>
/// <remarks>
/// Contexto adicional: dependencias, patrones aplicados, restricciones de uso.
/// </remarks>
public class MyClass { }
```

Use `<remarks>` when the class has notable dependencies injected, implements a specific pattern (e.g., MediatR handler, pipeline behavior), or has usage constraints the reader should know.

### Methods and handler `Handle` implementations

```csharp
/// <summary>
/// Descripción de lo que hace el método en una oración.
/// </summary>
/// <param name="paramName">Descripción del parámetro.</param>
/// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
/// <returns>Descripción del valor de retorno.</returns>
/// <exception cref="ExceptionType">Condición bajo la cual se lanza la excepción.</exception>
public async Task<Result<Guid>> Handle(MyCommand request, CancellationToken cancellationToken) { }
```

- Include `<param>` for every parameter.
- Include `<returns>` for every non-`void` method.
- Include `<exception cref="">` only when the method is documented to throw a specific exception type (infrastructure failures caught by `ExceptionMiddleware` do not need it).
- Always use the literal phrase `"Token para cancelar la operación asincrónica."` for `cancellationToken` parameters.

### Properties

Use the "Obtiene o establece" pattern for read/write properties and "Obtiene" for read-only properties:

```csharp
/// <summary>
/// Obtiene o establece el título del curso.
/// </summary>
public string? Title { get; set; }

/// <summary>
/// Obtiene el identificador único del recurso.
/// </summary>
public Guid Id { get; }
```

For navigation collection properties use this pattern:

```csharp
/// <summary>
/// Obtiene o establece la colección de [entidades relacionadas] asociada al [entidad actual].
/// </summary>
public ICollection<RelatedEntity>? RelatedEntities { get; set; }
```

### Constructors

Document constructors only when they perform non-trivial initialization or when the injected dependencies benefit from explanation:

```csharp
/// <summary>
/// Inicializa una nueva instancia de <see cref="MyClass"/> con las dependencias requeridas.
/// </summary>
/// <param name="dependency">Descripción de la dependencia inyectada.</param>
public MyClass(IDependency dependency) { }
```

### Interfaces

Document each interface and all its members. Interface `<summary>` describes the contract, not the implementation:

```csharp
/// <summary>
/// Define el contrato para [responsabilidad del servicio].
/// </summary>
public interface IMyService
{
    /// <summary>
    /// [Verbo] [descripción de la operación] de forma asíncrona.
    /// </summary>
    /// <param name="param">Descripción.</param>
    /// <returns>Descripción del resultado.</returns>
    Task<Result> DoSomethingAsync(string param);
}
```

---

## Layer-specific conventions

### Domain — entities and value types

- Document every entity class and all its properties.
- Use `<summary>` on the class to state what the entity represents in the domain.
- Do not document EF Core navigation properties with `<remarks>` — keep them brief with `<summary>` only.

```csharp
/// <summary>
/// Representa un curso de la plataforma con sus propiedades y colecciones de navegación.
/// </summary>
public class Course : BaseEntity { }
```

### Application — commands, queries, handlers, and validators

**Commands and queries (records):**

```csharp
/// <summary>
/// Comando para [acción] de [recurso].
/// </summary>
/// <param name="Request">Datos de entrada para [acción] el [recurso].</param>
public record CourseCreateCommand(CourseCreateRequest Request) : IRequest<Result<Guid>>;
```

**Handlers:**

- `<summary>` on the class describes the processing responsibility.
- `<remarks>` lists injected dependencies and the CQRS pattern applied.
- `<summary>` on `Handle` describes the operation performed, including side effects (e.g., photo upload, email notification).

**Validators:**

```csharp
/// <summary>
/// Validador para <see cref="CourseCreateRequest"/>. Define las reglas de validación de entrada.
/// </summary>
public class CourseCreateValidator : AbstractValidator<CourseCreateRequest> { }
```

### Infrastructure — service implementations

- `<summary>` on the class states which interface it implements and the external system it integrates with.
- `<exception cref="">` is expected on methods that wrap external SDK calls (e.g., Cloudinary).

```csharp
/// <summary>
/// Implementación de <see cref="IPhotoService"/> que gestiona fotos mediante Cloudinary.
/// </summary>
public class PhotoService : IPhotoService { }
```

### Persistence — DbContext and configurations

- Document `DbSet<T>` properties with a brief description of the represented table.
- Private helper methods (`LoadMasterData`, `LoadSecurityData`) must have `<summary>` tags.

```csharp
/// <summary>
/// Obtiene o establece el conjunto de entidades de cursos.
/// </summary>
public DbSet<Course>? Courses { get; set; }
```

### WebApi — controllers and middleware

**Controllers:**

- Class `<summary>` describes the resource managed and the available operations at a high level.
- Each action method must have `<summary>`, `<param>` for every parameter, and `<returns>` describing the success response and the possible error responses.

```csharp
/// <summary>
/// Controlador para la gestión de [recurso].
/// Proporciona endpoints para [listar/crear/actualizar/eliminar] [recurso].
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase { }

/// <summary>
/// Obtiene la lista paginada de cursos disponibles.
/// </summary>
/// <param name="pagingParams">Parámetros de paginación, filtrado y ordenamiento.</param>
/// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
/// <returns>Lista paginada de cursos o error de autorización.</returns>
[HttpGet]
public async Task<ActionResult<PagedList<CourseDto>>> GetCourses(...) { }
```

**Middleware:**

- Document the class describing the pipeline stage it handles.
- Document the `InvokeAsync` method describing the exceptions it catches and the HTTP responses it produces.

---

## `<see cref="">` and cross-references

Use `<see cref="">` to cross-reference related types instead of writing the type name as plain text:

```csharp
/// <summary>
/// Manejador para <see cref="CourseCreateCommand"/>. Persiste el curso en la base de datos
/// a través de <see cref="AppCoursesDbContext"/>.
/// </summary>
```

---

## What NOT to document

- Auto-generated migration files in `Persistence/Migrations/` — do not add XML comments.
- `Program.cs` — no documentation needed.
- Test setup files (when a test project is added) — follow a separate testing documentation guide.
- Obvious one-liners like `return Ok(result.Value)` — inline comments are not needed there.

---

## Common Spanish phrases for documentation

| Context | Phrase |
|---|---|
| Read/write property | `Obtiene o establece [el/la] [descripción].` |
| Read-only property | `Obtiene [el/la] [descripción].` |
| Async method | `[Verbo] [descripción] de forma asíncrona.` |
| CancellationToken param | `Token para cancelar la operación asincrónica.` |
| Command/query param | `Comando/consulta con los datos para [acción].` |
| Handler class | `Manejador para [CommandOrQuery]. [Descripción de responsabilidad].` |
| Interface contract | `Define el contrato para [responsabilidad].` |
| Controller class | `Controlador encargado de gestionar [recurso]. Proporciona endpoints para [acciones].` |
| Result return | `Resultado con [descripción del valor] o mensaje de error.` |
| Collection navigation | `Obtiene o establece la colección de [entidades] asociada[s] al/a la [entidad].` |

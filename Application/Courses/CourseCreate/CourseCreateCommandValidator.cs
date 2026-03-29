using FluentValidation;

namespace Application.Courses.CourseCreate
{
    /// <summary>
    /// Validador para el comando de creación de cursos.
    /// Valida los datos de entrada antes de procesar la creación de un nuevo curso.
    /// </summary>
    public class CourseCreateCommandValidator : AbstractValidator<CourseCreateCommand>
    {
        /// <summary>
        /// Inicializa una nueva instancia del validador de comando de creación de cursos.
        /// Configura las reglas de validación para el comando.
        /// </summary>
        public CourseCreateCommandValidator()
        {
            RuleFor(c => c.CourseCreateRequest).SetValidator(new CourseCreateValidator());
        }
    }
}
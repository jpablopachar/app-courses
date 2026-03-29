using FluentValidation;

namespace Application.Courses.CourseCreate
{
    /// <summary>
    /// Validador para la creación de cursos.
    /// Define las reglas de validación que debe cumplir una solicitud de creación de curso.
    /// </summary>
    public class CourseCreateValidator : AbstractValidator<CourseCreateRequest>
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CourseCreateValidator"/>.
        /// Configura las reglas de validación para los campos del curso.
        /// </summary>
        public CourseCreateValidator()
        {
            RuleFor(c => c.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(c => c.Description).NotEmpty().WithMessage("Description is required.");
        }
    }
}
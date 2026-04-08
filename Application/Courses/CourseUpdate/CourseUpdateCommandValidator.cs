using FluentValidation;

namespace Application.Courses.CourseUpdate
{
    /// <summary>
    /// Validador para el comando de actualización de cursos.
    /// Valida que la solicitud de actualización del curso sea válida y que el ID del curso no sea nulo.
    /// </summary>
    public class CourseUpdateCommandValidator : AbstractValidator<CourseUpdateCommand>
    {
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="CourseUpdateCommandValidator"/>.
        /// Configura las reglas de validación para el comando de actualización de cursos.
        /// </summary>
        public CourseUpdateCommandValidator()
        {
            // Valida que el objeto CourseUpdateRequest cumpla con las reglas del validador CourseUpdateValidator
            RuleFor(c => c.CourseUpdateRequest).SetValidator(new CourseUpdateValidator());
            
            // Valida que el ID del curso no sea nulo
            RuleFor(c => c.CourseId).NotNull();
        }
    }
}
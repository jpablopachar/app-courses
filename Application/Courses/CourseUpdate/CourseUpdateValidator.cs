using FluentValidation;

namespace Application.Courses.CourseUpdate
{
    /// <summary>
    /// Validador para las solicitudes de actualización de cursos.
    /// Define reglas de validación para los campos de un curso: título, descripción y fecha de publicación.
    /// </summary>
    public class CourseUpdateValidator : AbstractValidator<CourseUpdateRequest>
    {
        /// <summary>
        /// Inicializa una nueva instancia del validador CourseUpdateValidator.
        /// Configura las reglas de validación para los campos del curso.
        /// </summary>
        public CourseUpdateValidator()
        {
            RuleFor(c => c.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(c => c.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(c => c.PublicationDate).Must(ValidateDateTime).WithMessage("PublicationDate must be a valid date.");
        }

        /// <summary>
        /// Valida que la fecha de publicación sea válida.
        /// </summary>
        /// <param name="date">La fecha a validar. Puede ser nula.</param>
        /// <returns>Retorna true si la fecha es válida (no nula y no es la fecha por defecto); de lo contrario, false.</returns>
        private static bool ValidateDateTime(DateTime? date)
        {
            if (date == null) return false;
            if (date == default(DateTime)) return false;

            return true;
        }
    }
}
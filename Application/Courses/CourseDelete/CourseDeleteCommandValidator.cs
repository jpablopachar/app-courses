using FluentValidation;

namespace Application.Courses.CourseDelete
{
    /// <summary>
    /// Validador para el comando de eliminación de cursos.
    /// Define las reglas de validación que deben cumplir los datos del comando CourseDeleteCommand.
    /// </summary>
    public class CourseDeleteCommandValidator : AbstractValidator<CourseDeleteCommand>
    {
        /// <summary>
        /// Inicializa una nueva instancia del validador CourseDeleteCommandValidator.
        /// Configura las reglas de validación para el identificador del curso.
        /// </summary>
        public CourseDeleteCommandValidator()
        {
            RuleFor(c => c.CourseId).NotNull().WithMessage("CourseId is required.");
        }
    }
}
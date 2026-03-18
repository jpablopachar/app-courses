using FluentValidation;

namespace Application.Accounts.Register
{
    /// <summary>
    /// Validador para <see cref="RegisterCommand"/>.
    /// Asegura que la propiedad RegisterRequest sea válida utilizando <see cref="RegisterValidator"/>.
    /// </summary>
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RegisterCommandValidator"/>.
        /// Configura las reglas de validación para la propiedad RegisterRequest.
        /// </summary>
        public RegisterCommandValidator()
        {
            RuleFor(x => x.RegisterRequest).SetValidator(new RegisterValidator());
        }
    }
}

using FluentValidation;

namespace Application.Accounts.Login
{
    /// <summary>
    /// Validador para la clase <see cref="LoginCommand"/>.
    /// Asegura que la propiedad LoginRequest sea validada utilizando <see cref="LoginValidator"/>.
    /// </summary>
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="LoginCommandValidator"/>.
        /// Configura las reglas de validación para LoginCommand.
        /// </summary>
        public LoginCommandValidator()
        {
            RuleFor(x => x.LoginRequest).SetValidator(new LoginValidator());
        }
    }
}
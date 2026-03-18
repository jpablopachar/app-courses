using FluentValidation;

namespace Application.Accounts.Login
{
    /// <summary>
    /// Validador para la clase <see cref="LoginRequest"/>. Asegura que los campos de correo y contraseña cumplan con los criterios requeridos.
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="LoginValidator"/> y define las reglas de validación para solicitudes de inicio de sesión.
        /// </summary>
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
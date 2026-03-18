using FluentValidation;

namespace Application.Accounts.Register
{
    /// <summary>
    /// Validador para el modelo RegisterRequest. Asegura que todos los campos requeridos se proporcionen y sean válidos.
    /// </summary>
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RegisterValidator"/> y configura las reglas de validación para el registro.
        /// </summary>
        public RegisterValidator()
        {
            // Valida que FullName no esté vacío.
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name is required.");
            // Valida que Username no esté vacío.
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            // Valida que Email no esté vacío y sea una dirección de correo válida.
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            // Valida que Password no esté vacío y tenga una longitud mínima de 6 caracteres.
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            // Valida que Degree no esté vacío.
            RuleFor(x => x.Degree).NotEmpty().WithMessage("Degree is required.");
        }
    }
}
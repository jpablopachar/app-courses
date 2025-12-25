using FluentValidation;

namespace Application.Accounts.Register
{
    /// <summary>
    /// Validator for <see cref="RegisterCommand"/>.
    /// Ensures that the RegisterRequest property is valid using <see cref="RegisterValidator"/>.
    /// </summary>
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommandValidator"/> class.
        /// Sets up validation rules for the RegisterRequest property.
        /// </summary>
        public RegisterCommandValidator()
        {
            RuleFor(x => x.RegisterRequest).SetValidator(new RegisterValidator());
        }
    }
}

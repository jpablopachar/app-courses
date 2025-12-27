using FluentValidation;

namespace Application.Accounts.Login
{
    /// <summary>
    /// Validator for the <see cref="LoginCommand"/> class.
    /// Ensures that the LoginRequest property is validated using <see cref="LoginValidator"/>.
    /// </summary>
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandValidator"/> class.
        /// Sets up validation rules for the LoginCommand.
        /// </summary>
        public LoginCommandValidator()
        {
            RuleFor(x => x.LoginRequest).SetValidator(new LoginValidator());
        }
    }
}
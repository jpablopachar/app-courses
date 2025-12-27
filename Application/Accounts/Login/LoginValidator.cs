using FluentValidation;

namespace Application.Accounts.Login
{
    /// <summary>
    /// Validator for the <see cref="LoginRequest"/> class. Ensures that the email and password fields meet the required criteria.
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginValidator"/> class and defines validation rules for login requests.
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
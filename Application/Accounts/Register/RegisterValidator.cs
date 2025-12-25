using FluentValidation;

namespace Application.Accounts.Register
{
    /// <summary>
    /// Validator for the RegisterRequest model. Ensures that all required fields are provided and valid.
    /// </summary>
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterValidator"/> class and sets up validation rules for registration.
        /// </summary>
        public RegisterValidator()
        {
            // Validates that FullName is not empty.
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name is required.");
            // Validates that Username is not empty.
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            // Validates that Email is not empty and is a valid email address.
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            // Validates that Password is not empty and has a minimum length of 6 characters.
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            // Validates that Degree is not empty.
            RuleFor(x => x.Degree).NotEmpty().WithMessage("Degree is required.");
        }
    }
}
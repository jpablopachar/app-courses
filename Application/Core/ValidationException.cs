
using Application.Common.Constants;

namespace Application.Core
{
    /// <summary>
    /// Represents an exception that is thrown when validation errors occur.
    /// </summary>
    /// <remarks>
    /// This exception contains a collection of <see cref="ValidationError"/> objects describing the validation failures.
    /// </remarks>
    public sealed class ValidationException : Exception
    {
        /// <summary>
        /// Gets the collection of validation errors associated with this exception.
        /// </summary>
        public IReadOnlyList<ValidationError> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="errors">The collection of validation errors that caused the exception.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
        public ValidationException(IEnumerable<ValidationError> errors)
            : base(ErrorMessages.ValidationFailed)
        {
            ArgumentNullException.ThrowIfNull(errors);
            Errors = errors.ToList();
        }
    }
}
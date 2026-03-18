
using Application.Common.Constants;

namespace Application.Core
{
    /// <summary>
    /// Representa una excepción que se lanza cuando ocurren errores de validación.
    /// </summary>
    /// <remarks>
    /// Esta excepción contiene una colección de objetos <see cref="ValidationError"/> que describen los fallos de validación.
    /// </remarks>
    public sealed class ValidationException : Exception
    {
        /// <summary>
        /// Obtiene la colección de errores de validación asociados con esta excepción.
        /// </summary>
        public IReadOnlyList<ValidationError> Errors { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="errors">La colección de errores de validación que causaron la excepción.</param>
        /// <exception cref="ArgumentNullException">Se lanza cuando <paramref name="errors"/> es nulo.</exception>
        public ValidationException(IEnumerable<ValidationError> errors)
            : base(ErrorMessages.ValidationFailed)
        {
            ArgumentNullException.ThrowIfNull(errors);
            Errors = errors.ToList();
        }
    }
}
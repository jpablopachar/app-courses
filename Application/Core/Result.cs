namespace Application.Core
{
    /// <summary>
    /// Representa el resultado de una operación, indicando éxito o fracaso y conteniendo un valor o un mensaje de error.
    /// </summary>
    /// <typeparam name="T">El tipo del valor retornado por la operación.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Obtiene o establece un valor que indica si la operación fue exitosa.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Obtiene o establece el valor retornado por la operación si fue exitosa.
        /// </summary>
        public T? Value { get; set; }

        /// <summary>
        /// Obtiene o establece el mensaje de error si la operación falló.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Crea un resultado exitoso con el valor especificado.
        /// </summary>
        /// <param name="value">El valor que se retornará como parte del resultado exitoso.</param>
        /// <returns>Una instancia de <see cref="Result{T}"/> que representa éxito.</returns>
        public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };

        /// <summary>
        /// Crea un resultado fallido con el mensaje de error especificado.
        /// </summary>
        /// <param name="error">El mensaje de error que describe el fallo.</param>
        /// <returns>Una instancia de <see cref="Result{T}"/> que representa fracaso.</returns>
        public static Result<T> Failure(string error) => new() { IsSuccess = false, Error = error };
    }
}
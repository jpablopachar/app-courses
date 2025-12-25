namespace Application.Core
{
    /// <summary>
    /// Represents the result of an operation, indicating success or failure and containing a value or an error message.
    /// </summary>
    /// <typeparam name="T">The type of the value returned by the operation.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the value returned by the operation if it was successful.
        /// </summary>
        public T? Value { get; set; }

        /// <summary>
        /// Gets or sets the error message if the operation failed.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Creates a successful result with the specified value.
        /// </summary>
        /// <param name="value">The value to be returned as part of the successful result.</param>
        /// <returns>A <see cref="Result{T}"/> instance representing success.</returns>
        public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };

        /// <summary>
        /// Creates a failed result with the specified error message.
        /// </summary>
        /// <param name="error">The error message describing the failure.</param>
        /// <returns>A <see cref="Result{T}"/> instance representing failure.</returns>
        public static Result<T> Failure(string error) => new() { IsSuccess = false, Error = error };
    }
}
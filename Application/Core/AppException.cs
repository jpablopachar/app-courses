namespace Application.Core
{
    /// <summary>
    /// Represents an application-specific exception with HTTP status code, message, and optional details.
    /// </summary>
    public class AppException(int statusCode, string? message = null, string? details = null)
    {
        /// <summary>
        /// Gets or sets the HTTP status code associated with the exception.
        /// </summary>
        public int StatusCode { get; set; } = statusCode;

        /// <summary>
        /// Gets or sets the error message describing the exception.
        /// </summary>
        public string? Message { get; set; } = message;

        /// <summary>
        /// Gets or sets additional details about the exception.
        /// </summary>
        public string? Details { get; set; } = details;
    }
}
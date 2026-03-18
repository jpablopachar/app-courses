namespace Application.Core
{
    /// <summary>
    /// Representa una excepción específica de la aplicación con código de estado HTTP, mensaje y detalles opcionales.
    /// </summary>
    public class AppException(int statusCode, string? message = null, string? details = null)
    {
        /// <summary>
        /// Obtiene o establece el código de estado HTTP asociado a la excepción.
        /// </summary>
        public int StatusCode { get; set; } = statusCode;

        /// <summary>
        /// Obtiene o establece el mensaje de error que describe la excepción.
        /// </summary>
        public string? Message { get; set; } = message;

        /// <summary>
        /// Obtiene o establece detalles adicionales sobre la excepción.
        /// </summary>
        public string? Details { get; set; } = details;
    }
}
using Application.Core;
using Newtonsoft.Json;

namespace WebApi.Middlewares
{
    /// <summary>
    /// Middleware para manejar excepciones globales en la aplicación.
    /// Captura todas las excepciones no controladas y las convierte en respuestas JSON estandarizadas.
    /// </summary>
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        /// <summary>
        /// Invoca el middleware para procesar la solicitud y manejar cualquier excepción que ocurra.
        /// </summary>
        /// <param name="context">Contexto HTTP de la solicitud actual.</param>
        /// <returns>Una tarea que representa la ejecución asincrónica del middleware.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                var response = ex switch
                {
                    ValidationException validationException => new AppException(StatusCodes.Status400BadRequest, "Error of validation", string.Join(",", validationException.Errors.Select(error => error.ErrorMessage))),

                    _ => new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                };

                context.Response.StatusCode = response.StatusCode;
                context.Response.ContentType = "application/json";

                var json = JsonConvert.SerializeObject(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
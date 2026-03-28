using Microsoft.OpenApi.Models;

namespace WebApi.Extensions
{
    /// <summary>
    /// Clase de extensión para configurar la documentación de Swagger/OpenAPI en la aplicación.
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        /// <summary>
        /// Array que contiene los esquemas de seguridad soportados.
        /// </summary>
        private static readonly string[] value = ["Bearer"];

        /// <summary>
        /// Extiende IServiceCollection para agregar la documentación de Swagger a la aplicación.
        /// Configura el esquema de seguridad JWT Bearer y los requisitos de seguridad para OpenAPI.
        /// </summary>
        /// <param name="services">Colección de servicios de la aplicación.</param>
        /// <returns>La colección de servicios para encadenamiento de métodos.</returns>
        public static IServiceCollection AddSwaggerDocumentation
    (
        this IServiceCollection services
    )
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization Bearer Schema",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    securitySchema, value }
            };

                c.AddSecurityRequirement(securityRequirement);

            });


            return services;
        }

        /// <summary>
        /// Extiende IApplicationBuilder para configurar el middleware de Swagger en la aplicación.
        /// Habilita la interfaz de usuario de Swagger para explorar la documentación de la API.
        /// </summary>
        /// <param name="app">Constructor de aplicación de la aplicación.</param>
        /// <returns>El constructor de aplicación para encadenamiento de métodos.</returns>
        public static IApplicationBuilder UseSwaggerDocumentation(
            this IApplicationBuilder app
        )
        {

            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
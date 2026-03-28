using Application.Core;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    /// <summary>
    /// Clase estática que configura la inyección de dependencias para la capa de aplicación.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Método de extensión que registra los servicios de la capa de aplicación en el contenedor de inyección de dependencias.
        /// </summary>
        /// <param name="services">La colección de servicios a la que se agregarán las dependencias.</param>
        /// <returns>La colección de servicios configurada para permitir encadenamiento de métodos.</returns>
        /// <remarks>
        /// Este método realiza las siguientes operaciones:
        /// <list type="bullet">
        /// <item><description>Registra los manejadores de MediatR desde el ensamblado actual.</description></item>
        /// <item><description>Agrega el comportamiento de validación personalizado para todas las solicitudes.</description></item>
        /// <item><description>Registra los validadores de FluentValidation desde el ensamblado actual.</description></item>
        /// <item><description>Configura AutoMapper con los perfiles de mapeo disponibles.</description></item>
        /// </list>
        /// </remarks>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            return services;
        }
    }
}
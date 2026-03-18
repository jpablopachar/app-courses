using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    /// <summary>
    /// Proporciona métodos de extensión para registrar servicios de la capa de infraestructura en el contenedor de inyección de dependencias.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Agrega los servicios de la capa de infraestructura al contenedor de inyección de dependencias de la aplicación.
        /// </summary>
        /// <param name="services">El IServiceCollection al que se agregan los servicios.</param>
        /// <returns>El IServiceCollection actualizado.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<IQualificationRepository, QualificationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}

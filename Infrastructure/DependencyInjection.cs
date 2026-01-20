using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering infrastructure layer services in the dependency injection container.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the infrastructure layer services to the application's dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<IQualificationRepository, QualificationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}

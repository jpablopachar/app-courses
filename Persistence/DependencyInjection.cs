using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    /// <summary>
    /// Provides extension methods for registering persistence layer services in the dependency injection container.
    /// </summary>
    public static class DependencyInjection
    {
        private const string SqliteConnectionStringName = "SqliteDb";

        /// <summary>
        /// Adds the persistence layer services to the application's dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The application configuration containing connection strings.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration,
            bool isDevelopment = false
        )
        {
            services.AddDbContext<AppCoursesDbContext>(opt =>
            {
                if (isDevelopment)
                {
                    opt.LogTo(Console.WriteLine, [
                        DbLoggerCategory.Database.Command.Name
                    ], LogLevel.Information).EnableSensitiveDataLogging();
                    opt.UseSqlite(configuration.GetConnectionString(SqliteConnectionStringName));
                }
            });

            return services;
        }
    }
}
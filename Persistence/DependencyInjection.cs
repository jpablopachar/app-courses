using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    /// <summary>
    /// Proporciona métodos de extensión para registrar servicios de la capa de persistencia en el contenedor de inyección de dependencias.
    /// </summary>
    public static class DependencyInjection
    {
        private const string SqliteConnectionStringName = "SqliteDb";

        /// <summary>
        /// Agrega los servicios de la capa de persistencia al contenedor de inyección de dependencias de la aplicación.
        /// </summary>
        /// <param name="services">El IServiceCollection al que se agregan los servicios.</param>
        /// <param name="configuration">La configuración de la aplicación que contiene las cadenas de conexión.</param>
        /// <returns>El IServiceCollection actualizado.</returns>
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<AppCoursesDbContext>(opt =>
            {
                opt.LogTo(Console.WriteLine, [
                    DbLoggerCategory.Database.Command.Name
                ], LogLevel.Information).EnableSensitiveDataLogging();

                opt.UseSqlite(configuration.GetConnectionString(SqliteConnectionStringName));
            });

            return services;
        }
    }
}
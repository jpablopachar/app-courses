using System.Text;
using Application.Interfaces;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Models;

namespace WebApi.Extensions
{
    /// <summary>
    /// Clase de extensión para configurar los servicios de identidad y autenticación JWT.
    /// </summary>
    public static class IdentityServiceExtensions
    {
        /// <summary>
        /// Extiende la colección de servicios para agregar y configurar la autenticación con JWT,
        /// gestión de identidad de usuarios y roles.
        /// </summary>
        /// <param name="services">La colección de servicios donde se agregarán las instancias.</param>
        /// <param name="config">La configuración de la aplicación que contiene la clave de tokens.</param>
        /// <returns>La colección de servicios configurada para permitir encadenamiento de métodos.</returns>
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppCoursesDbContext>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserAccessor, IUserAccessor>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]!));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }
    }
}
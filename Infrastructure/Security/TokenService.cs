using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Models;

namespace Infrastructure.Security
{
    /// <summary>
    /// Proporciona funcionalidad para generar tokens JWT para los usuarios de la aplicación.
    /// </summary>
    /// <remarks>
    /// Este servicio recupera las políticas de usuario desde la base de datos y las incluye como roles en el token generado.
    /// </remarks>
    public class TokenService(AppCoursesDbContext appCoursesDbContext, IConfiguration configuration) : ITokenService
    {
        private readonly AppCoursesDbContext _context = appCoursesDbContext;
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Crea un token JWT para el usuario especificado, incluyendo sus roles como claims.
        /// </summary>
        /// <param name="user">El usuario de la aplicación para el que se creará el token.</param>
        /// <returns>Un token JWT como cadena.</returns>
        public async Task<string> CreateToken(AppUser user)
        {
            var policies = await _context.Database.SqlQuery<string>(
                $@"SELECT DISTINCT anrc.ClaimValue
                FROM AspNetUserRoles anur
                INNER JOIN AspNetRoleClaims anrc ON anur.RoleId = anrc.RoleId 
                WHERE anur.UserId = {user.Id}
                AND anrc.ClaimValue IS NOT NULL"
            ).ToListAsync();

            var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Name, user.UserName!),
                    new(ClaimTypes.Email, user.Email!)
                };

            foreach (var policy in policies)
            {
                if (policy is not null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, policy));
                }
            }

            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]!)), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
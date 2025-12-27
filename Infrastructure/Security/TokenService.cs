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
    /// Provides functionality for generating JWT tokens for application users.
    /// </summary>
    /// <remarks>
    /// This service retrieves user policies from the database and includes them as roles in the generated token.
    /// </remarks>
    public class TokenService(AppCoursesDbContext appCoursesDbContext, IConfiguration configuration) : ITokenService
    {
        private readonly AppCoursesDbContext _context = appCoursesDbContext;
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Creates a JWT token for the specified user, including their roles as claims.
        /// </summary>
        /// <param name="user">The application user for whom the token is to be created.</param>
        /// <returns>A JWT token as a string.</returns>
        public async Task<string> CreateToken(AppUser user)
        {
            // TODO: Refactor to use proper EF Core querying instead of raw SQL
            var policies = await _context.Database.SqlQuery<string>($"SELECT p.Name FROM Policies p INNER JOIN AppUserPolicies ap ON p.Id = ap.PolicyId WHERE ap.UserId = '{user.Id}'").ToListAsync();

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
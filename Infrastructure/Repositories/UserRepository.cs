using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementa operaciones de acceso a datos de usuarios utilizando ASP.NET Core Identity UserManager.
    /// </summary>
    /// <remarks>
    /// Inicializa una nueva instancia de la clase <see cref="UserRepository"/>.
    /// </remarks>
    /// <param name="userManager">La instancia de UserManager para acceder a los datos de usuario.</param>
    public class UserRepository(UserManager<AppUser> userManager) : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        /// <inheritdoc/>
        public async Task<AppUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<UserExistsResult?> FindUserByEmailOrUsernameAsync(string email, string username, CancellationToken cancellationToken = default)
        {
            return await _userManager.Users
                .Where(u => u.Email == email || u.UserName == username)
                .Select(u => new UserExistsResult
                {
                    Email = u.Email,
                    UserName = u.UserName
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

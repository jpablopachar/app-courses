using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implements user data access operations using ASP.NET Core Identity UserManager.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </remarks>
    /// <param name="userManager">The UserManager instance for accessing user data.</param>
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

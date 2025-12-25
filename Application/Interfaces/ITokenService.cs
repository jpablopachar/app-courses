using Persistence.Models;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides functionality for creating authentication tokens for users.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token will be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the generated JWT token as a string.</returns>
        Task<string> CreateToken(AppUser user);
    }
}
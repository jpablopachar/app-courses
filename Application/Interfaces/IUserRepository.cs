using Persistence.Models;

namespace Application.Interfaces
{
    /// <summary>
    /// Defines the contract for user data access operations.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The user with the specified email, or null if not found.</returns>
        Task<AppUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a user exists with the specified email or username.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <param name="username">The username to check.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An anonymous object with Email and UserName properties if found, null otherwise.</returns>
        Task<UserExistsResult?> FindUserByEmailOrUsernameAsync(string email, string username, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents the result of checking if a user exists by email or username.
    /// </summary>
    public class UserExistsResult
    {
        /// <summary>
        /// Gets or sets the email of the existing user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the username of the existing user.
        /// </summary>
        public string? UserName { get; set; }
    }
}

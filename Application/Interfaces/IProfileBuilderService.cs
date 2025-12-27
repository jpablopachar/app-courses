using Application.Accounts;
using Persistence.Models;

namespace Application.Interfaces
{
    /// <summary>
    /// Provides functionality to build a profile for a given application user.
    /// </summary>
    public interface IProfileBuilderService
    {
        /// <summary>
        /// Builds a profile asynchronously for the specified application user.
        /// </summary>
        /// <param name="user">The application user for whom the profile will be built.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the built <see cref="Profile"/>.</returns>
        Task<Profile> BuildProfileAsync(AppUser user);
    }
}
using Application.Accounts;
using Application.Interfaces;

namespace Infrastructure.Accounts
{
    /// <summary>
    /// Service responsible for building user profiles, including generating authentication tokens.
    /// </summary>
    public class ProfileBuilderService(ITokenService tokenService) : IProfileBuilderService
    {
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Builds a <see cref="Profile"/> for the specified user, including a generated authentication token.
        /// </summary>
        /// <param name="user">The user for whom to build the profile.</param>
        /// <returns>A <see cref="Profile"/> object containing user information and an authentication token.</returns>
        public async Task<Profile> BuildProfileAsync(Persistence.Models.AppUser user)
        {
            return new Profile
            {
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }
    }
}
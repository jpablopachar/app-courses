using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Application.Accounts.GetCurrentUser
{
    /// <summary>
    /// Handles the query to get the current user's profile information.
    /// </summary>
    /// <remarks>
    /// Uses UserManager to retrieve user data and ITokenService to generate a JWT token.
    /// </remarks>
    /// <param name="userManager">The UserManager instance for accessing user data.</param>
    /// <param name="tokenService">The token service for generating JWT tokens.</param>
    public class GetCurrentUserQueryHandler(UserManager<AppUser> userManager, ITokenService tokenService) : IRequestHandler<GetCurrentUserQuery, Result<Profile>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Handles the GetCurrentUserQuery request and returns the user's profile.
        /// </summary>
        /// <param name="request">The query containing the user's email.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A Result containing the user's profile or an error message.</returns>
        public async Task<Result<Profile>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.GetCurrentUserRequest.Email, cancellationToken);

            if (user is null) return Result<Profile>.Failure("User not found");

            var profile = new Profile
            {
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };

            return Result<Profile>.Success(profile);
        }
    }
}
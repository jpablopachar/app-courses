using Application.Common.Constants;
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
    /// <param name="profileBuilderService">The profile builder service for creating user profiles.</param>
    public class GetCurrentUserQueryHandler(UserManager<AppUser> userManager, IProfileBuilderService profileBuilderService) : IRequestHandler<GetCurrentUserQuery, Result<Profile>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IProfileBuilderService _profileBuilderService = profileBuilderService;

        /// <summary>
        /// Handles the GetCurrentUserQuery request and returns the user's profile.
        /// </summary>
        /// <param name="request">The query containing the user's email.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A Result containing the user's profile or an error message.</returns>
        public async Task<Result<Profile>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.GetCurrentUserRequest.Email, cancellationToken);

            if (user is null) return Result<Profile>.Failure(ErrorMessages.UserNotFound);

            var profile = await _profileBuilderService.BuildProfileAsync(user);

            return Result<Profile>.Success(profile);
        }
    }
}
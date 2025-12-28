using Application.Common.Constants;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Accounts.GetCurrentUser
{
    /// <summary>
    /// Handles the query to get the current user's profile information.
    /// </summary>
    /// <remarks>
    /// Uses IUserRepository to retrieve user data and IProfileBuilderService to generate a profile with JWT token.
    /// </remarks>
    /// <param name="userRepository">The user repository for accessing user data.</param>
    /// <param name="profileBuilderService">The profile builder service for creating user profiles.</param>
    public class GetCurrentUserQueryHandler(IUserRepository userRepository, IProfileBuilderService profileBuilderService) : IRequestHandler<GetCurrentUserQuery, Result<Profile>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IProfileBuilderService _profileBuilderService = profileBuilderService;

        /// <summary>
        /// Handles the GetCurrentUserQuery request and returns the user's profile.
        /// </summary>
        /// <param name="request">The query containing the user's email.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A Result containing the user's profile or an error message.</returns>
        public async Task<Result<Profile>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.GetCurrentUserRequest.Email!, cancellationToken);

            if (user is null) return Result<Profile>.Failure(ErrorMessages.UserNotFound);

            var profile = await _profileBuilderService.BuildProfileAsync(user);

            return Result<Profile>.Success(profile);
        }
    }
}
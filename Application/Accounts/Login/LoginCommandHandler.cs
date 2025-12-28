using Application.Common.Constants;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Models;

namespace Application.Accounts.Login
{
    /// <summary>
    /// Handles the login command by validating user credentials and generating a profile with a JWT token.
    /// </summary>
    /// <remarks>
    /// This handler uses IUserRepository to retrieve user data and UserManager to verify credentials.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
    /// </remarks>
    /// <param name="userRepository">The user repository for accessing user data.</param>
    /// <param name="userManager">The user manager for handling password verification.</param>
    /// <param name="profileBuilderService">The profile builder service for creating user profiles.</param>
    public class LoginCommandHandler(IUserRepository userRepository, UserManager<AppUser> userManager, IProfileBuilderService profileBuilderService) : IRequestHandler<LoginCommand, Result<Profile>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IProfileBuilderService _profileBuilderService = profileBuilderService;

        /// <summary>
        /// Handles the login request by validating the user's credentials and returning a profile with a JWT token if successful.
        /// </summary>
        /// <param name="request">The login command containing the login request data.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A <see cref="Result{Profile}"/> containing the user's profile and token if successful, or an error message if authentication fails.</returns>
        public async Task<Result<Profile>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.LoginRequest.Email!, cancellationToken);

            if (user is null) return Result<Profile>.Failure(ErrorMessages.UserNotFound);

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.LoginRequest.Password!);

            if (!passwordValid) return Result<Profile>.Failure(ErrorMessages.InvalidCredentials);

            var profile = await _profileBuilderService.BuildProfileAsync(user);

            return Result<Profile>.Success(profile);
        }
    }
}
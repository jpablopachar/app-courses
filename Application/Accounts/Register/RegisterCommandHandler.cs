using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Application.Accounts.Register
{

    /// <summary>
    /// Handles user registration by validating email and username uniqueness, creating a new user,
    /// and generating an authentication token for the registered user.
    /// </summary>
    /// <remarks>
    /// This handler uses <see cref="UserManager{AppUser}"/> to manage user creation and <see cref="ITokenService"/> to generate tokens.
    /// </remarks>
    /// <param name="userManager">The user manager for handling user-related operations.</param>
    /// <param name="profileBuilderService">The profile builder service for creating user profiles.</param>
    public class RegisterCommandHandler(UserManager<AppUser> userManager, IProfileBuilderService profileBuilderService) : IRequestHandler<RegisterCommand, Result<Profile>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IProfileBuilderService _profileBuilderService = profileBuilderService;

        /// <summary>
        /// Handles the registration of a new user by validating email and username uniqueness, creating the user,
        /// and returning a profile with a generated authentication token.
        /// </summary>
        /// <param name="request">The register command containing user registration details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Result{Profile}"/> containing the user's profile and token if successful, or an error message if registration fails.</returns>
        public async Task<Result<Profile>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.Users.Where(u => u.Email == request.RegisterRequest.Email || u.UserName == request.RegisterRequest.Username).Select(u => new { u.Email, u.UserName }).FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.Email == request.RegisterRequest.Email)
                {
                    return Result<Profile>.Failure("Email is already taken");
                }

                return Result<Profile>.Failure("Username is already taken");
            }


            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = request.RegisterRequest.FullName,
                UserName = request.RegisterRequest.Username,
                Email = request.RegisterRequest.Email,
                Occupation = request.RegisterRequest.Degree
            };

            var result = await _userManager.CreateAsync(user, request.RegisterRequest.Password!);

            if (!result.Succeeded)
            {
                return Result<Profile>.Failure("Failed to register user");
            }

            var profile = await _profileBuilderService.BuildProfileAsync(user);

            return Result<Profile>.Success(profile);
        }
    }
}
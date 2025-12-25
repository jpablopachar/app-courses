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
    /// <param name="tokenService">The token service for generating authentication tokens.</param>
    public class RegisterCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService) : IRequestHandler<RegisterCommand, Result<Profile>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Handles the registration of a new user by validating email and username uniqueness, creating the user,
        /// and returning a profile with a generated authentication token.
        /// </summary>
        /// <param name="request">The register command containing user registration details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Result{Profile}"/> containing the user's profile and token if successful, or an error message if registration fails.</returns>
        public async Task<Result<Profile>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.RegisterRequest.Email, cancellationToken))
            {
                return Result<Profile>.Failure("Email is already taken");
            }

            if (await _userManager.Users.AnyAsync(x => x.UserName == request.RegisterRequest.Username, cancellationToken))
            {
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
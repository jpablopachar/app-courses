using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Application.Accounts.Login
{
    /// <summary>
    /// Handles the login command by validating user credentials and generating a profile with a JWT token.
    /// </summary>
    /// <remarks>
    /// This handler uses ASP.NET Core Identity to verify the user's email and password, and generates a JWT token upon successful authentication.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
    /// </remarks>
    /// <param name="userManager">The user manager for handling user operations.</param>
    /// <param name="tokenService">The token service for generating JWT tokens.</param>
    public class LoginCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService) : IRequestHandler<LoginCommand, Result<Profile>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Handles the login request by validating the user's credentials and returning a profile with a JWT token if successful.
        /// </summary>
        /// <param name="request">The login command containing the login request data.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A <see cref="Result{Profile}"/> containing the user's profile and token if successful, or an error message if authentication fails.</returns>
        public async Task<Result<Profile>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.LoginRequest.Email, cancellationToken);

            if (user is null) return Result<Profile>.Failure("User not found");

            var result = await _userManager.CheckPasswordAsync(user, request.LoginRequest.Password!);

            if (!result) return Result<Profile>.Failure("Invalid email or password");

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
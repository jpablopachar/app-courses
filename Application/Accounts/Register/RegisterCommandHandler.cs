using Application.Common.Constants;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Application.Accounts.Register
{
    public class RegisterCommandHandler(UserManager<AppUser> userManager, IProfileBuilderService profileBuilderService)
        : IRequestHandler<RegisterCommand, Result<Profile>>
    {
        public async Task<Result<Profile>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await userManager.Users.AnyAsync(u => u.Email == request.RegisterRequest.Email, cancellationToken))
            {
                return Result<Profile>.Failure(ErrorMessages.EmailTaken);
            }

            if (await userManager.Users.AnyAsync(u => u.UserName == request.RegisterRequest.Username,
                    cancellationToken))
            {
                return Result<Profile>.Failure(ErrorMessages.UsernameTaken);
            }

            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = request.RegisterRequest.FullName,
                UserName = request.RegisterRequest.Username,
                Email = request.RegisterRequest.Email,
                Occupation = request.RegisterRequest.Degree
            };

            var result = await userManager.CreateAsync(user, request.RegisterRequest.Password!);

            if (!result.Succeeded)
            {
                return Result<Profile>.Failure(ErrorMessages.RegistrationFailed);
            }

            var profile = await profileBuilderService.BuildProfileAsync(user);

            return Result<Profile>.Success(profile);
        }
    }
}
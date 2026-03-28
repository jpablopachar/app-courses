using Application.Common.Constants;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Application.Accounts.Register
{
    /// <summary>
    /// Controlador que maneja el proceso de registro de nuevos usuarios en la aplicación.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa el patrón CQRS (Command Query Responsibility Segregation) mediante
    /// MediatR, permitiendo procesar comandos de registro de forma asincrónica.
    /// Valida que el email y nombre de usuario no estén registrados antes de crear el usuario.
    /// </remarks>
    public class RegisterCommandHandler(UserManager<AppUser> userManager, IProfileBuilderService profileBuilderService)
        : IRequestHandler<RegisterCommand, Result<Profile>>
    {
        /// <summary>
        /// Maneja el registro de un nuevo usuario en la aplicación.
        /// </summary>
        /// <param name="request">Comando que contiene los datos de registro del usuario.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
        /// <returns>Un resultado que contiene el perfil creado si el registro es exitoso, o un mensaje de error en caso contrario.</returns>
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
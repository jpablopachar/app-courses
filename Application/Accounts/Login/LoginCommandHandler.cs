using Application.Common.Constants;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Application.Accounts.Login
{
    /// <summary>
    /// Controlador de comando para el inicio de sesión de usuarios.
    /// Implementa la lógica de autenticación validando las credenciales del usuario
    /// y construyendo su perfil si la autenticación es exitosa.
    /// </summary>
    public class LoginCommandHandler(UserManager<AppUser> userManager, IProfileBuilderService profileBuilderService)
        : IRequestHandler<LoginCommand, Result<Profile>>
    {
        /// <summary>
        /// Maneja la solicitud de inicio de sesión validando las credenciales del usuario.
        /// </summary>
        /// <param name="request">Comando de inicio de sesión que contiene el email y contraseña.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
        /// <returns>Resultado con el perfil del usuario si las credenciales son válidas; de lo contrario, un error.</returns>
        public async Task<Result<Profile>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == request.LoginRequest.Email,
                cancellationToken);

            if (user is null) return Result<Profile>.Failure(ErrorMessages.UserNotFound);

            var passwordValid = await userManager.CheckPasswordAsync(user, request.LoginRequest.Password!);

            if (!passwordValid) return Result<Profile>.Failure(ErrorMessages.InvalidCredentials);

            var profile = await profileBuilderService.BuildProfileAsync(user);

            return Result<Profile>.Success(profile);
        }
    }
}
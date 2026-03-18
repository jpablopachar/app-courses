using Application.Common.Constants;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Models;

namespace Application.Accounts.Login
{
    /// <summary>
    /// Maneja el comando de inicio de sesión validando las credenciales del usuario y generando un perfil con un token JWT.
    /// </summary>
    /// <remarks>
    /// Este manejador utiliza IUserRepository para obtener datos del usuario y UserManager para verificar credenciales.
    /// </remarks>
    /// <remarks>
    /// Inicializa una nueva instancia de la clase <see cref="LoginCommandHandler"/>.
    /// </remarks>
    /// <param name="userRepository">El repositorio de usuarios para acceder a los datos del usuario.</param>
    /// <param name="userManager">El gestor de usuarios para gestionar la verificación de contraseñas.</param>
    /// <param name="profileBuilderService">El servicio generador de perfiles para crear perfiles de usuarios.</param>
    public class LoginCommandHandler(IUserRepository userRepository, UserManager<AppUser> userManager, IProfileBuilderService profileBuilderService) : IRequestHandler<LoginCommand, Result<Profile>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IProfileBuilderService _profileBuilderService = profileBuilderService;

        /// <summary>
        /// Maneja la solicitud de inicio de sesión validando las credenciales del usuario y retornando un perfil con un token JWT si es exitoso.
        /// </summary>
        /// <param name="request">El comando de inicio de sesión que contiene los datos de la solicitud de inicio de sesión.</param>
        /// <param name="cancellationToken">Un token de cancelación.</param>
        /// <returns>Un <see cref="Result{Profile}"/> que contiene el perfil del usuario y token si es exitoso, o un mensaje de error si la autenticación falla.</returns>
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
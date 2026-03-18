using Application.Common.Constants;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Models;

namespace Application.Accounts.Register
{

    /// <summary>
    /// Maneja el registro de usuarios validando la unicidad del correo y nombre de usuario, creando un nuevo usuario,
    /// y generando un token de autenticación para el usuario registrado.
    /// </summary>
    /// <remarks>
    /// Este manejador utiliza <see cref="IUserRepository"/> para verificar usuarios existentes y <see cref="UserManager{AppUser}"/> para gestionar la creación de usuarios.
    /// </remarks>
    /// <param name="userRepository">El repositorio de usuarios para verificar la existencia del usuario.</param>
    /// <param name="userManager">El gestor de usuarios para gestionar operaciones de creación de usuarios.</param>
    /// <param name="profileBuilderService">El servicio generador de perfiles para crear perfiles de usuarios.</param>
    public class RegisterCommandHandler(IUserRepository userRepository, UserManager<AppUser> userManager, IProfileBuilderService profileBuilderService) : IRequestHandler<RegisterCommand, Result<Profile>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IProfileBuilderService _profileBuilderService = profileBuilderService;

        /// <summary>
        /// Maneja el registro de un nuevo usuario validando la unicidad del correo y nombre de usuario, creando el usuario,
        /// y retornando un perfil con un token de autenticación generado.
        /// </summary>
        /// <param name="request">El comando de registro que contiene los detalles del registro del usuario.</param>
        /// <param name="cancellationToken">Un token para monitorear solicitudes de cancelación.</param>
        /// <returns>Un <see cref="Result{Profile}"/> que contiene el perfil del usuario y token si es exitoso, o un mensaje de error si el registro falla.</returns>
        public async Task<Result<Profile>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.FindUserByEmailOrUsernameAsync(
                request.RegisterRequest.Email!,
                request.RegisterRequest.Username!,
                cancellationToken
            );

            if (existingUser != null)
            {
                if (existingUser.Email == request.RegisterRequest.Email)
                {
                    return Result<Profile>.Failure(ErrorMessages.EmailTaken);
                }

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

            var result = await _userManager.CreateAsync(user, request.RegisterRequest.Password!);

            if (!result.Succeeded)
            {
                return Result<Profile>.Failure(ErrorMessages.RegistrationFailed);
            }

            var profile = await _profileBuilderService.BuildProfileAsync(user);

            return Result<Profile>.Success(profile);
        }
    }
}
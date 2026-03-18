using Application.Common.Constants;
using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Accounts.GetCurrentUser
{
    /// <summary>
    /// Maneja la consulta para obtener la información del perfil del usuario actual.
    /// </summary>
    /// <remarks>
    /// Utiliza IUserRepository para obtener datos del usuario e IProfileBuilderService para generar un perfil con token JWT.
    /// </remarks>
    /// <param name="userRepository">El repositorio de usuarios para acceder a los datos del usuario.</param>
    /// <param name="profileBuilderService">El servicio generador de perfiles para crear perfiles de usuarios.</param>
    public class GetCurrentUserQueryHandler(IUserRepository userRepository, IProfileBuilderService profileBuilderService) : IRequestHandler<GetCurrentUserQuery, Result<Profile>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IProfileBuilderService _profileBuilderService = profileBuilderService;

        /// <summary>
        /// Maneja la solicitud GetCurrentUserQuery y retorna el perfil del usuario.
        /// </summary>
        /// <param name="request">La consulta que contiene el correo del usuario.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Un Result que contiene el perfil del usuario o un mensaje de error.</returns>
        public async Task<Result<Profile>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.GetCurrentUserRequest.Email!, cancellationToken);

            if (user is null) return Result<Profile>.Failure(ErrorMessages.UserNotFound);

            var profile = await _profileBuilderService.BuildProfileAsync(user);

            return Result<Profile>.Success(profile);
        }
    }
}
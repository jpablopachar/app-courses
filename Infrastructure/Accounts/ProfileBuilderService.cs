using Application.Accounts;
using Application.Interfaces;

namespace Infrastructure.Accounts
{
    /// <summary>
    /// Servicio responsable de construir perfiles de usuario, incluyendo la generación de tokens de autenticación.
    /// </summary>
    public class ProfileBuilderService(ITokenService tokenService) : IProfileBuilderService
    {
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Construye un <see cref="Profile"/> para el usuario especificado, incluyendo un token de autenticación generado.
        /// </summary>
        /// <param name="user">El usuario para el que se construye el perfil.</param>
        /// <returns>Un objeto <see cref="Profile"/> que contiene la información del usuario y un token de autenticación.</returns>
        public async Task<Profile> BuildProfileAsync(Persistence.Models.AppUser user)
        {
            return new Profile
            {
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }
    }
}
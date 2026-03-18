using Persistence.Models;

namespace Application.Interfaces
{
    /// <summary>
    /// Proporciona funcionalidad para crear tokens de autenticación para usuarios.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Crea un token JWT para el usuario especificado.
        /// </summary>
        /// <param name="user">El usuario para el que se creará el token.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea contiene el token JWT generado como cadena.</returns>
        Task<string> CreateToken(AppUser user);
    }
}
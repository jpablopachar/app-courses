using Persistence.Models;

namespace Application.Interfaces
{
    /// <summary>
    /// Define el contrato para operaciones de acceso a datos de usuarios.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Obtiene un usuario por su dirección de correo electrónico.
        /// </summary>
        /// <param name="email">La dirección de correo del usuario.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>El usuario con el correo especificado, o null si no se encuentra.</returns>
        Task<AppUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica si existe un usuario con el correo o nombre de usuario especificado.
        /// </summary>
        /// <param name="email">El correo a verificar.</param>
        /// <param name="username">El nombre de usuario a verificar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Un objeto con propiedades Email y UserName si se encuentra, null en caso contrario.</returns>
        Task<UserExistsResult?> FindUserByEmailOrUsernameAsync(string email, string username, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Representa el resultado de verificar si un usuario existe por correo o nombre de usuario.
    /// </summary>
    public class UserExistsResult
    {
        /// <summary>
        /// Obtiene o establece el correo del usuario existente.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de usuario del usuario existente.
        /// </summary>
        public string? UserName { get; set; }
    }
}

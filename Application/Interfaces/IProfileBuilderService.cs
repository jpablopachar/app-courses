using Application.Accounts;
using Persistence.Models;

namespace Application.Interfaces
{
    /// <summary>
    /// Proporciona funcionalidad para construir un perfil para un usuario de la aplicación dado.
    /// </summary>
    public interface IProfileBuilderService
    {
        /// <summary>
        /// Construye un perfil de manera asíncrona para el usuario de la aplicación especificado.
        /// </summary>
        /// <param name="user">El usuario de la aplicación para quien se construirá el perfil.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea contiene el <see cref="Profile"/> construido.</returns>
        Task<Profile> BuildProfileAsync(AppUser user);
    }
}
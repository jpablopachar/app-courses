using Microsoft.AspNetCore.Identity;

namespace Persistence.Models
{
    /// <summary>
    /// Representa un usuario de la aplicación con información adicional de perfil.
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// Obtiene o establece el nombre completo del usuario.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Obtiene o establece la ocupación del usuario.
        /// </summary>
        public string? Occupation { get; set; }
    }
}
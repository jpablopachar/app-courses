namespace Application.Accounts
{
    /// <summary>
    /// Representa un perfil de usuario con información básica de la cuenta.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Obtiene o establece el nombre completo del usuario.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Obtiene o establece la dirección de correo del usuario.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Obtiene o establece el token de autenticación del usuario.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de usuario del usuario.
        /// </summary>
        public string? Username { get; set; }
    }
}
namespace Application.Accounts.Login
{
    /// <summary>
    /// Representa una solicitud de inicio de sesión de usuario que contiene correo y contraseña.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Obtiene o establece la dirección de correo del usuario que intenta iniciar sesión.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario que intenta iniciar sesión.
        /// </summary>
        public string? Password { get; set; }
    }
}
namespace Application.Accounts.Register
{
    /// <summary>
    /// Representa una solicitud para registrar una nueva cuenta.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Obtiene o establece el nombre completo del usuario.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de usuario.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Obtiene o establece la dirección de correo electrónico del usuario.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Obtiene o establece el grado del usuario.
        /// </summary>
        public string? Degree { get; set; }
    }
}
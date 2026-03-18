namespace Domain
{
    /// <summary>
    /// Proporciona nombres de roles personalizados utilizados para la autorización dentro de la aplicación.
    /// </summary>
    public static class CustomRoles
    {
        /// <summary>
        /// Representa el rol de administrador.
        /// </summary>
        public const string ADMIN = nameof(ADMIN);

        /// <summary>
        /// Representa el rol de cliente.
        /// </summary>
        public const string CLIENT = nameof(CLIENT);
    }
}
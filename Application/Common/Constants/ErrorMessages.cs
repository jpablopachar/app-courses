namespace Application.Common.Constants
{
    /// <summary>
    /// Proporciona mensajes de error constantes utilizados en toda la aplicación.
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// Mensaje de error cuando no se encuentra un usuario.
        /// </summary>
        public const string UserNotFound = "User not found";

        /// <summary>
        /// Mensaje de error para credenciales de correo o contraseña inválidas.
        /// </summary>
        public const string InvalidCredentials = "Invalid email or password";

        /// <summary>
        /// Mensaje de error cuando el correo ya está en uso.
        /// </summary>
        public const string EmailTaken = "Email is already taken";

        /// <summary>
        /// Mensaje de error cuando el nombre de usuario ya está en uso.
        /// </summary>
        public const string UsernameTaken = "Username is already taken";

        /// <summary>
        /// Mensaje de error cuando falla el registro de usuario.
        /// </summary>
        public const string RegistrationFailed = "Failed to register user";

        /// <summary>
        /// Mensaje de error cuando la validación falla.
        /// </summary>
        public const string ValidationFailed = "One or more validation errors occurred.";
    }
}
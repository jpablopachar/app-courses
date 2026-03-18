namespace Application.Accounts.GetCurrentUser
{
    /// <summary>
    /// Representa una solicitud para obtener la información del usuario actual.
    /// </summary>
    public class GetCurrentUserRequest
    {
        /// <summary>
        /// Obtiene o establece la dirección de correo del usuario actual.
        /// </summary>
        public string? Email { get; set; }
    }
}
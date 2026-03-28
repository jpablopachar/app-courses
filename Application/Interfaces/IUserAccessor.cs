namespace Application.Interfaces
{
    /// <summary>
    /// Interfaz para acceder a la información del usuario actual.
    /// </summary>
    public interface IUserAccessor
    {
        /// <summary>
        /// Obtiene el nombre de usuario del usuario actual.
        /// </summary>
        /// <returns>El nombre de usuario.</returns>
        string GetUsername();

        /// <summary>
        /// Obtiene el correo electrónico del usuario actual.
        /// </summary>
        /// <returns>El correo electrónico.</returns>
        string GetEmail();
    }
}
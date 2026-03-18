namespace Application.Core
{
    /// <summary>
    /// Representa un error de validación para una propiedad específica.
    /// </summary>
    /// <param name="PropertyName">El nombre de la propiedad que falló la validación.</param>
    /// <param name="ErrorMessage">El mensaje de error que describe el fallo de validación.</param>
    public sealed record ValidationError(string PropertyName, string ErrorMessage);
}
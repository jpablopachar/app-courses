namespace Infrastructure.Photos
{
    /// <summary>
    /// Configuración para la integración con Cloudinary.
    /// Contiene las credenciales necesarias para autenticar y utilizar los servicios de Cloudinary.
    /// </summary>
    public class CloudinarySettings
    {
        /// <summary>
        /// Obtiene o establece el nombre de la nube de Cloudinary.
        /// </summary>
        public string? CloudName { get; set; }
        
        /// <summary>
        /// Obtiene o establece la clave de API de Cloudinary.
        /// </summary>
        public string? ApiKey { get; set; }
        
        /// <summary>
        /// Obtiene o establece el secreto de API de Cloudinary.
        /// </summary>
        public string? ApiSecret { get; set; }
    }
}
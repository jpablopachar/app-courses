namespace Application.Photos
{
    /// <summary>
    /// Resultado de la carga de una foto.
    /// </summary>
    public class PhotoUploadResult
    {
        /// <summary>
        /// Identificador público de la foto en el servicio de almacenamiento.
        /// </summary>
        public string? PublicId { get; set; }

        /// <summary>
        /// URL de acceso a la foto cargada.
        /// </summary>
        public string? Url { get; set; }
    }
}
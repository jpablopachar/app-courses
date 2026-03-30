namespace Application.Photos
{
    /// <summary>
    /// DTO de respuesta para obtener la información de una foto asociada a un curso.
    /// </summary>
    /// <param name="Id">Identificador único de la foto.</param>
    /// <param name="Url">URL pública de la foto.</param>
    /// <param name="CourseId">Identificador del curso al que pertenece la foto.</param>
    public record PhotoResponse(
        Guid? Id,
        string? Url,
        Guid? CourseId
    )
    {
        public PhotoResponse() : this(null, null, null)
        {
        }
    }
}
namespace Domain
{
    /// <summary>
    /// Representa una entidad de foto asociada a un curso.
    /// </summary>
    public class Photo : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece la URL de la foto.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del curso asociado.
        /// </summary>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Obtiene o establece el curso asociado a esta foto.
        /// </summary>
        public Course? Course { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador público de la foto (por ejemplo, para almacenamiento en la nube).
        /// </summary>
        public string? PublicId { get; set; }
    }
}
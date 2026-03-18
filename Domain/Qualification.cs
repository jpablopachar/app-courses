namespace Domain
{
    /// <summary>
    /// Representa un registro de calificación para un estudiante, incluyendo puntaje, comentarios e información del curso asociado.
    /// </summary>
    public class Qualification : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el nombre del estudiante asociado a esta calificación.
        /// </summary>
        public string? Student { get; set; }
        /// <summary>
        /// Obtiene o establece el puntaje obtenido por el estudiante.
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// Obtiene o establece un comentario opcional sobre la calificación.
        /// </summary>
        public string? Comment { get; set; }
        /// <summary>
        /// Obtiene o establece el identificador único del curso asociado.
        /// </summary>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Obtiene o establece el curso asociado a esta calificación.
        /// </summary>
        public Course? Course { get; set; }
    }
}
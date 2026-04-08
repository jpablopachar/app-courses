namespace Application.Courses.CourseUpdate
{
    /// <summary>
    /// Solicitud para actualizar los datos de un curso.
    /// </summary>
    public class CourseUpdateRequest
    {
        /// <summary>
        /// Obtiene o establece el título del curso.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del curso.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de publicación del curso.
        /// </summary>
        public DateTime? PublicationDate { get; set; }
    }
}
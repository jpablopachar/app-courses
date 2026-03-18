namespace Domain
{
    /// <summary>
    /// Representa una entidad de curso con propiedades relacionadas y colecciones de navegación.
    /// </summary>
    public class Course : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el título del curso.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del curso.
        /// </summary>
        public string? Description
        {
            get;
            set;
        }

        /// <summary>
        /// Obtiene o establece la fecha de publicación del curso.
        /// </summary>
        public DateTime? PublicationDate { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de calificaciones asociadas al curso.
        /// </summary>
        public ICollection<Qualification>? Qualifications { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de precios asociada al curso.
        /// </summary>
        public ICollection<Price>? Prices { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de precios de curso asociada al curso.
        /// </summary>
        public ICollection<CoursePrice>? CoursePrices { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de instructores asociada al curso.
        /// </summary>
        public ICollection<Instructor>? Instructors { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de relaciones curso-instructor asociada al curso.
        /// </summary>
        public ICollection<CourseInstructor>? CourseInstructors { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de fotos asociada al curso.
        /// </summary>
        public ICollection<Photo>? Photos { get; set; }
    }
}
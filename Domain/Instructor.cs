namespace Domain
{
    /// <summary>
    /// Representa una entidad de instructor con información personal y académica.
    /// </summary>
    public class Instructor : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el apellido del instructor.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del instructor.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Obtiene o establece el grado académico del instructor.
        /// </summary>
        public string? Degree { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de cursos del instructor.
        /// </summary>
        public ICollection<Course>? Courses { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de relaciones curso-instructor.
        /// </summary>
        public ICollection<CourseInstructor>? CourseInstructors { get; set; }
    }
}
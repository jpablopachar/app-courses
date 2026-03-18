namespace Domain
{
    /// <summary>
    /// Representa la asociación entre un curso y un instructor.
    /// </summary>
    public class CourseInstructor
    {
        /// <summary>
        /// Obtiene o establece el identificador único del curso.
        /// </summary>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Obtiene o establece el curso asociado al instructor.
        /// </summary>
        public Course? Course { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador único del instructor.
        /// </summary>
        public Guid? InstructorId { get; set; }

        /// <summary>
        /// Obtiene o establece el instructor asociado al curso.
        /// </summary>
        public Instructor? Instructor { get; set; }
    }
}

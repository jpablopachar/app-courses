namespace Domain
{
    /// <summary>
    /// Represents the association between a course and an instructor.
    /// </summary>
    public class CourseInstructor
    {
        /// <summary>
        /// Gets or sets the unique identifier of the course.
        /// </summary>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Gets or sets the course associated with the instructor.
        /// </summary>
        public Course? Course { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the instructor.
        /// </summary>
        public Guid? InstructorId { get; set; }

        /// <summary>
        /// Gets or sets the instructor associated with the course.
        /// </summary>
        public Instructor? Instructor { get; set; }
    }
}

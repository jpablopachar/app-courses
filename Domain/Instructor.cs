namespace Domain
{
    /// <summary>
    /// Represents an instructor entity with personal and academic information.
    /// </summary>
    public class Instructor : BaseEntity
    {
        /// <summary>
        /// Gets or sets the last name of the instructor.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name of the instructor.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the academic degree of the instructor.
        /// </summary>
        public string? Degree { get; set; }

        /// <summary>
        /// Gets or sets the academic institution of the instructor.
        /// </summary>
        public ICollection<Course>? Courses { get; set; }

        /// <summary>
        /// Gets or sets the collection of course-instructor relationships.
        /// </summary>
        public ICollection<CourseInstructor>? CourseInstructors { get; set; }
    }
}
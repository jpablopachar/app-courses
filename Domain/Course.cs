namespace Domain
{
    /// <summary>
    /// Represents a course entity with related properties and navigation collections.
    /// </summary>
    public class Course : BaseEntity
    {
        /// <summary>
        /// Gets or sets the title of the course.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the course.
        /// </summary>
        public string? Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the publication date of the course.
        /// </summary>
        public DateTime? PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of qualifications associated with the course.
        /// </summary>
        public ICollection<Qualification>? Qualifications { get; set; }

        /// <summary>
        /// Gets or sets the collection of prices associated with the course.
        /// </summary>
        public ICollection<Price>? Prices { get; set; }

        /// <summary>
        /// Gets or sets the collection of course prices associated with the course.
        /// </summary>
        public ICollection<CoursePrice>? CoursePrices { get; set; }

        /// <summary>
        /// Gets or sets the collection of instructors associated with the course.
        /// </summary>
        public ICollection<Instructor>? Instructors { get; set; }

        /// <summary>
        /// Gets or sets the collection of course instructors associated with the course.
        /// </summary>
        public ICollection<CourseInstructor>? CourseInstructors { get; set; }

        /// <summary>
        /// Gets or sets the collection of photos associated with the course.
        /// </summary>
        public ICollection<Photo>? Photos { get; set; }
    }
}
namespace Domain
{
    /// <summary>
    /// Represents a qualification record for a student, including score, comments, and associated course information.
    /// </summary>
    public class Qualification : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the student associated with this qualification.
        /// </summary>
        public string? Student { get; set; }
        /// <summary>
        /// Gets or sets the score achieved by the student.
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// Gets or sets an optional comment regarding the qualification.
        /// </summary>
        public string? Comment { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier of the associated course.
        /// </summary>
        public Guid? CourseId { get; set; }
    }
}
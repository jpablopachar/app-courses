namespace Domain
{
    /// <summary>
    /// Represents a photo entity associated with a course.
    /// </summary>
    public class Photo : BaseEntity
    {
        /// <summary>
        /// Gets or sets the URL of the photo.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated course.
        /// </summary>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Gets or sets the course associated with this photo.
        /// </summary>
        public Course? Course { get; set; }

        /// <summary>
        /// Gets or sets the public identifier of the photo (e.g., for cloud storage).
        /// </summary>
        public string? PublicId { get; set; }
    }
}
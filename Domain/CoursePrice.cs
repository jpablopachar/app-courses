namespace Domain
{
    /// <summary>
    /// Represents the association between a Course and its Price.
    /// </summary>
    public class CoursePrice
    {
        /// <summary>
        /// Gets or sets the unique identifier of the associated Course.
        /// </summary>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Gets or sets the Course entity associated with this CoursePrice.
        /// </summary>
        public Course? Course { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated Price.
        /// </summary>
        public Guid? PriceId { get; set; }

        /// <summary>
        /// Gets or sets the Price entity associated with this CoursePrice.
        /// </summary>
        public Price? Price { get; set; }
    }
}
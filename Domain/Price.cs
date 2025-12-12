namespace Domain
{
    /// <summary>
    /// Represents the price details for a course, including current and promotional prices.
    /// </summary>
    public class Price : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the price.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the current price value.
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Gets or sets the promotional price value.
        /// </summary>
        public decimal PromotionalPrice { get; set; }

        /// <summary>
        /// Gets or sets the collection of courses associated with this price.
        /// </summary>
        public ICollection<Course>? Courses { get; set; }

        /// <summary>
        /// Gets or sets the collection of course-price relationships.
        /// </summary>
        public ICollection<CoursePrice>? CoursePrices { get; set; }
    }
}
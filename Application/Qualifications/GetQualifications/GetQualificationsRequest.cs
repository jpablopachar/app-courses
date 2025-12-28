using Application.Core;

namespace Application.Qualifications.GetQualifications
{
    /// <summary>
    /// Request parameters for retrieving qualifications with optional filtering and paging.
    /// </summary>
    public class GetQualificationsRequest : PagingParams
    {
        /// <summary>
        /// Gets or sets the student identifier or name to filter qualifications.
        /// </summary>
        public string? Student { get; set; }

        /// <summary>
        /// Gets or sets the course identifier to filter qualifications.
        /// </summary>
        public Guid? CourseId { get; set; }
    }
}
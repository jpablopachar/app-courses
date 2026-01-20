using Application.Core;

namespace Application.Instructors.GetInstructors
{
    /// <summary>
    /// Request parameters for retrieving a paginated list of instructors, with optional filtering by name and last name.
    /// </summary>
    public class GetInstructorsRequest : PagingParams
    {
        /// <summary>
        /// Optional filter for the instructor's first name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Optional filter for the instructor's last name.
        /// </summary>
        public string? LastName { get; set; }
    }
}
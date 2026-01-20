using Application.Core;
using MediatR;

namespace Application.Instructors.GetInstructors
{
    /// <summary>
    /// Query to retrieve a paginated list of instructors based on the specified request parameters.
    /// </summary>
    /// <remarks>
    /// This record is used to encapsulate the parameters for fetching instructors, supporting pagination and filtering.
    /// </remarks>
    public record GetInstructorsQuery : IRequest<Result<PagedList<InstructorResponse>>>
    {
        /// <summary>
        /// The request parameters for retrieving instructors, including pagination and filtering options.
        /// </summary>
        public GetInstructorsRequest? GetInstructorsRequest { get; init; }
    }
}
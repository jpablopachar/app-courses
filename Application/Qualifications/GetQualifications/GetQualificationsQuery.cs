using Application.Core;
using MediatR;

namespace Application.Qualifications.GetQualifications
{
    /// <summary>
    /// Query to retrieve a paged list of qualifications based on the specified request parameters.
    /// </summary>
    /// <remarks>
    /// This query is used to request qualifications with optional filtering and paging options.
    /// </remarks>
    public record GetQualificationsQuery
        : IRequest<Result<PagedList<QualificationResponse>>>
    {
        /// <summary>
        /// The request parameters for filtering and paging qualifications.
        /// </summary>
        public GetQualificationsRequest? QualificationsRequest { get; set; }
    }
}
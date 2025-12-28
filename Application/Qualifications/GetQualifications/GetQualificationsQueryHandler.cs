using System.Linq.Expressions;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;

namespace Application.Qualifications.GetQualifications
{
    /// <summary>
    /// Handles queries for retrieving paginated qualifications with filtering and sorting.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetQualificationsQueryHandler"/> class.
    /// </remarks>
    /// <param name="repository">The qualification repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public class GetQualificationsQueryHandler(IQualificationRepository repository, IMapper mapper) : IRequestHandler<GetQualificationsQuery, Result<PagedList<QualificationResponse>>>
    {
        private readonly IQualificationRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc/>
        public async Task<Result<PagedList<QualificationResponse>>> Handle(
            GetQualificationsQuery request,
            CancellationToken cancellationToken)
        {
            var requestParams = request.QualificationsRequest!;
            var predicate = BuildFilterPredicate(requestParams);
            var orderBySelector = BuildOrderBySelector(requestParams.OrderBy);
            var orderAscending = requestParams.OrderAsc ?? true;

            var qualifications = _repository.GetQualifications(
                predicate,
                orderBySelector,
                orderAscending
            );

            var qualificationResponses = qualifications
                .ProjectTo<QualificationResponse>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var pagedList = await PagedList<QualificationResponse>.CreateAsync(
                qualificationResponses,
                requestParams.PageNumber,
                requestParams.PageSize
            );

            return Result<PagedList<QualificationResponse>>.Success(pagedList);
        }

        /// <summary>
        /// Builds a filter predicate based on the request parameters.
        /// </summary>
        /// <param name="requestParams">The request parameters containing filter criteria.</param>
        /// <returns>An expression representing the filter predicate.</returns>
        private static Expression<Func<Qualification, bool>> BuildFilterPredicate(GetQualificationsRequest requestParams)
        {
            var predicate = ExpressionBuilder.New<Qualification>();

            if (!string.IsNullOrEmpty(requestParams.Student))
            {
                predicate = predicate.And(q => q.Student != null && q.Student.Contains(requestParams.Student));
            }

            if (requestParams.CourseId.HasValue)
            {
                predicate = predicate.And(q => q.CourseId == requestParams.CourseId);
            }

            return predicate;
        }

        /// <summary>
        /// Builds an order by selector based on the specified field name.
        /// </summary>
        /// <param name="orderByField">The field name to order by.</param>
        /// <returns>An expression representing the order by selector, or null if no ordering is specified.</returns>
        private static Expression<Func<Qualification, object>>? BuildOrderBySelector(string? orderByField)
        {
            if (string.IsNullOrEmpty(orderByField))
            {
                return null;
            }

            return orderByField.ToLower() switch
            {
                "student" => q => q.Student ?? string.Empty,
                "score" => q => q.Score,
                _ => q => q.Student ?? string.Empty
            };
        }
    }
}
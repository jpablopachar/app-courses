using System.Linq.Expressions;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Persistence;

namespace Application.Qualifications.GetQualifications
{
    public class GetQualificationsQueryHandler(AppCoursesDbContext context, IMapper mapper) : IRequestHandler<GetQualificationsQuery, Result<PagedList<QualificationResponse>>>
    {
        private readonly AppCoursesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PagedList<QualificationResponse>>> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Qualification> queryable = _context.Qualifications!;

            var predicate = ExpressionBuilder.New<Qualification>();

            if (!string.IsNullOrEmpty(request.QualificationsRequest!.Student))
            {
                predicate = predicate.And(q => q.Student!.Contains(request.QualificationsRequest.Student));
            }

            if (request.QualificationsRequest.CourseId is not null)
            {
                predicate = predicate.And(q => q.CourseId == request.QualificationsRequest.CourseId);
            }

            if (!string.IsNullOrEmpty(request.QualificationsRequest.OrderBy))
            {
                Expression<Func<Qualification, object>>? orderBySelector = request.QualificationsRequest.OrderBy.ToLower() switch
                {
                    "student" => q => q.Student!,
                    "score" => q => q.Score,
                    _ => q => q.Student!
                };

                bool orderBy = request.QualificationsRequest.OrderAsc ?? true;

                queryable = orderBy
                    ? queryable.OrderBy(orderBySelector)
                    : queryable.OrderByDescending(orderBySelector);
            }

            queryable = queryable.Where(predicate);

            var qualificationQuery = queryable
                .ProjectTo<QualificationResponse>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var pagination = PagedList<QualificationResponse>.CreateAsync(
                qualificationQuery,
                request.QualificationsRequest!.PageNumber,
                request.QualificationsRequest.PageSize);

            return Result<PagedList<QualificationResponse>>.Success(await pagination);
        }
    }
}
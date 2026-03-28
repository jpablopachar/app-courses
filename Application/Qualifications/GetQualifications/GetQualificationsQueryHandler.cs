using System.Linq.Expressions;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Persistence;

namespace Application.Qualifications.GetQualifications
{
    /// <summary>
    /// Maneja consultas para obtener calificaciones paginadas con filtrado y ordenamiento.
    /// </summary>
    /// <remarks>
    /// Inicializa una nueva instancia de la clase <see cref="GetQualificationsQueryHandler"/>.
    /// </remarks>
    /// <param name="repository">El repositorio de calificaciones.</param>
    /// <param name="mapper">La instancia de AutoMapper.</param>
    public class GetQualificationsQueryHandler(AppCoursesDbContext context, IMapper mapper) : IRequestHandler<GetQualificationsQuery, Result<PagedList<QualificationResponse>>>
    {
        private readonly AppCoursesDbContext _context = context;
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

            var qualifications = _context.Qualifications!
                .Where(predicate);

            if (orderBySelector is not null)
            {
                qualifications = orderAscending
                    ? qualifications.OrderBy(orderBySelector)
                    : qualifications.OrderByDescending(orderBySelector);
            }

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
        /// Construye un predicado de filtro basado en los parámetros de solicitud.
        /// </summary>
        /// <param name="requestParams">Los parámetros de solicitud que contienen criterios de filtro.</param>
        /// <returns>Una expresión que representa el predicado de filtro.</returns>
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
        /// Construye un selector de ordenamiento basado en el nombre de campo especificado.
        /// </summary>
        /// <param name="orderByField">El nombre del campo para ordenar.</param>
        /// <returns>Una expresión que representa el selector de ordenamiento, o null si no se especifica ordenamiento.</returns>
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
using System.Linq.Expressions;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;

namespace Application.Instructors.GetInstructors
{
    /// <summary>
    /// Maneja consultas para obtener instructores paginados con filtrado y ordenamiento.
    /// </summary>
    /// <remarks>
    /// Inicializa una nueva instancia de la clase <see cref="GetInstructorsQueryHandler"/>.
    /// </remarks>
    /// <param name="repository">El repositorio de instructores.</param>
    /// <param name="mapper">La instancia de AutoMapper.</param>
    public class GetInstructorsQueryHandler(IInstructorRepository repository, IMapper mapper) : IRequestHandler<GetInstructorsQuery, Result<PagedList<InstructorResponse>>>
    {
        private readonly IInstructorRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc/>
        public async Task<Result<PagedList<InstructorResponse>>> Handle(
            GetInstructorsQuery request,
            CancellationToken cancellationToken)
        {
            var requestParams = request.GetInstructorsRequest!;
            var predicate = BuildFilterPredicate(requestParams);
            var orderBySelector = BuildOrderBySelector(requestParams.OrderBy);
            var orderAscending = requestParams.OrderAsc ?? true;

            var instructors = _repository.GetInstructors(
                predicate,
                orderBySelector,
                orderAscending
            );

            var instructorResponses = instructors
                .ProjectTo<InstructorResponse>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var pagedList = await PagedList<InstructorResponse>.CreateAsync(
                instructorResponses,
                requestParams.PageNumber,
                requestParams.PageSize
            );

            return Result<PagedList<InstructorResponse>>.Success(pagedList);
        }

        /// <summary>
        /// Construye un predicado de filtro basado en los parámetros de solicitud.
        /// </summary>
        /// <param name="requestParams">Los parámetros de solicitud que contienen criterios de filtro.</param>
        /// <returns>Una expresión que representa el predicado de filtro.</returns>
        private static Expression<Func<Instructor, bool>> BuildFilterPredicate(GetInstructorsRequest requestParams)
        {
            var predicate = ExpressionBuilder.New<Instructor>();

            if (!string.IsNullOrEmpty(requestParams.Name))
            {
                predicate = predicate.And(p => p.Name!.Contains(requestParams.Name));
            }

            if (!string.IsNullOrEmpty(requestParams.LastName))
            {
                predicate = predicate.And(p => p.LastName!.Contains(requestParams.LastName));
            }

            return predicate;
        }

        /// <summary>
        /// Builds an order by selector based on the specified field name.
        /// </summary>
        /// <param name="orderByField">The field name to order by.</param>
        /// <returns>An expression representing the order by selector, or null if no ordering is specified.</returns>
        private static Expression<Func<Instructor, object>>? BuildOrderBySelector(string? orderByField)
        {
            if (string.IsNullOrEmpty(orderByField))
            {
                return null;
            }

            return orderByField.ToLower() switch
            {
                "name" => instructor => instructor.Name!,
                "lastname" => instructor => instructor.LastName!,
                _ => instructor => instructor.Name!
            };
        }
    }
}

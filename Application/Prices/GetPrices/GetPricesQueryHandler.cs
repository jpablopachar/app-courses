using System.Linq.Expressions;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Persistence;

namespace Application.Prices.GetPrices
{
    /// <summary>
    /// Maneja consultas para obtener precios paginados con filtrado y ordenamiento.
    /// </summary>
    /// <remarks>
    /// Inicializa una nueva instancia de la clase <see cref="GetPricesQueryHandler"/>.
    /// </remarks>
    /// <param name="context">El contexto de base de datos de AppCourses.</param>
    /// <param name="mapper">La instancia de AutoMapper.</param>
    public class GetPricesQueryHandler(AppCoursesDbContext context, IMapper mapper) : IRequestHandler<GetPricesQuery, Result<PagedList<PriceResponse>>>
    {
        private readonly AppCoursesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc/>
        public async Task<Result<PagedList<PriceResponse>>> Handle(GetPricesQuery request, CancellationToken cancellationToken)
        {
            var requestParams = request.PricesRequest!;
            var predicate = BuildFilterPredicate(requestParams);
            var orderBySelector = BuildOrderBySelector(requestParams.OrderBy);
            var orderAscending = requestParams.OrderAsc ?? true;

            var prices = _context.Prices!
                .Where(predicate);

            if (orderBySelector is not null)
            {
                prices = orderAscending
                    ? prices.OrderBy(orderBySelector)
                    : prices.OrderByDescending(orderBySelector);
            }

            var priceResponses = prices
                .ProjectTo<PriceResponse>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var pagedList = await PagedList<PriceResponse>.CreateAsync(
                priceResponses,
                requestParams.PageNumber,
                requestParams.PageSize
            );

            return Result<PagedList<PriceResponse>>.Success(pagedList);
        }

        /// <summary>
        /// Construye un predicado de filtro basado en los parámetros de solicitud.
        /// </summary>
        /// <param name="requestParams">Los parámetros de solicitud que contienen criterios de filtro.</param>
        /// <returns>Una expresión que representa el predicado de filtro.</returns>
        private static Expression<Func<Price, bool>> BuildFilterPredicate(GetPricesRequest requestParams)
        {
            var predicate = ExpressionBuilder.New<Price>();

            if (!string.IsNullOrEmpty(requestParams.Name))
            {
                predicate = predicate.And(p => p.Name!.Contains(requestParams.Name));
            }

            return predicate;
        }

        /// <summary>
        /// Construye un selector de ordenamiento basado en el nombre de campo especificado.
        /// </summary>
        /// <param name="orderByField">El nombre del campo para ordenar.</param>
        /// <returns>Una expresión que representa el selector de ordenamiento, o null si no se especifica ordenamiento.</returns>
        private static Expression<Func<Price, object>>? BuildOrderBySelector(string? orderByField)
        {
            if (string.IsNullOrEmpty(orderByField))
            {
                return null;
            }

            return orderByField.ToLower() switch
            {
                "name" => p => p.Name!,
                "price" => p => p.CurrentPrice!,
                _ => p => p.Name!
            };
        }
    }
}
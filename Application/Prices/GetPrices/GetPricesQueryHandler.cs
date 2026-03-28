using System.Linq.Expressions;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Persistence;

namespace Application.Prices.GetPrices
{
    public class GetPricesQueryHandler(AppCoursesDbContext context, IMapper mapper) : IRequestHandler<GetPricesQuery, Result<PagedList<PriceResponse>>>
    {
        private readonly AppCoursesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PagedList<PriceResponse>>> Handle(GetPricesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Price> queryable = _context.Prices!;

            var predicate = ExpressionBuilder.New<Price>();

            if (!string.IsNullOrEmpty(request.PricesRequest?.Name))
            {
                predicate = predicate.Or(p => p.Name!.Contains(request.PricesRequest.Name));
            }

            if (!string.IsNullOrEmpty(request.PricesRequest!.OrderBy))
            {
                Expression<Func<Price, object>>? orderSelector = request.PricesRequest.OrderBy.ToLower() switch
                {
                    "name" => p => p.Name!,
                    "price" => p => p.CurrentPrice!,
                    _ => p => p.Name!
                };

                bool orderBy = request.PricesRequest.OrderAsc ?? true;

                queryable = orderBy ? queryable.OrderBy(orderSelector) : queryable.OrderByDescending(orderSelector);
            }

            queryable = queryable.Where(predicate);

            var queryPrices = queryable.ProjectTo<PriceResponse>(_mapper.ConfigurationProvider).AsQueryable();

            var pagination = await PagedList<PriceResponse>.CreateAsync(queryPrices, request.PricesRequest!.PageNumber, request.PricesRequest.PageSize);

            return Result<PagedList<PriceResponse>>.Success(pagination);
        }
    }
}
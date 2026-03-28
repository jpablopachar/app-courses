using Application.Core;
using MediatR;

namespace Application.Prices.GetPrices
{
    /// <summary>
    /// Consulta para obtener la lista de precios con paginación.
    /// </summary>
    public record GetPricesQuery : IRequest<Result<PagedList<PriceResponse>>>
    {
        /// <summary>
        /// Obtiene o establece la solicitud de precios con parámetros de filtrado y paginación.
        /// </summary>
        public GetPricesRequest? PricesRequest { get; set; }
    }
}
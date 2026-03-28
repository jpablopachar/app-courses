using Application.Core;

namespace Application.Prices.GetPrices
{
    /// <summary>
    /// Solicitud para obtener precios con parámetros de paginación y filtrado.
    /// </summary>
    public class GetPricesRequest : PagingParams
    {
        /// <summary>
        /// Obtiene o establece el nombre del producto para filtrar los precios.
        /// </summary>
        public string? Name { get; set; }
    }
}
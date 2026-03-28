namespace Application.Prices.GetPrices
{
    /// <summary>
    /// Representa la respuesta con información de precios.
    /// </summary>
    /// <param name="Id">Identificador único del precio.</param>
    /// <param name="Name">Nombre del producto o servicio.</param>
    /// <param name="CurrentAmount">Monto actual del precio.</param>
    /// <param name="PromotionalAmount">Monto promocional del precio.</param>
    public record PriceResponse(Guid? Id, string? Name, decimal? CurrentAmount, decimal? PromotionalAmount)
    {
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="PriceResponse"/> con valores nulos.
        /// </summary>
        public PriceResponse() : this(null, null, null, null)
        {
        }
    }
}
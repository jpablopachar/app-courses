using Microsoft.EntityFrameworkCore;

namespace Application.Core
{
    /// <summary>
    /// Representa una lista paginada de elementos con metadatos de paginación.
    /// </summary>
    /// <typeparam name="T">El tipo de elementos en la lista.</typeparam>
    /// <param name="items">Los elementos de la página actual.</param>
    /// <param name="count">El número total de elementos.</param>
    /// <param name="pageNumber">El número de página actual.</param>
    /// <param name="pageSize">El número de elementos por página.</param>
    public class PagedList<T>(
    List<T> items,
    int count,
    int pageNumber,
    int pageSize
    )
    {
        /// <summary>
        /// Obtiene o establece el número de página actual.
        /// </summary>
        public int CurrentPage { get; set; } = pageNumber;

        /// <summary>
        /// Obtiene o establece el número total de páginas.
        /// </summary>
        public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);

        /// <summary>
        /// Obtiene o establece el número de elementos por página.
        /// </summary>
        public int PageSize { get; set; } = pageSize;

        /// <summary>
        /// Obtiene o establece el número total de elementos.
        /// </summary>
        public int TotalCount { get; set; } = count;

        /// <summary>
        /// Obtiene o establece los elementos de la página actual.
        /// </summary>
        public List<T> Items { get; set; } = items;

        /// <summary>
        /// Crea una lista paginada de manera asíncrona desde una fuente <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <param name="source">La fuente de datos consultable.</param>
        /// <param name="pageNumber">El número de página a recuperar.</param>
        /// <param name="pageSize">El número de elementos por página.</param>
        /// <returns>Una instancia de <see cref="PagedList{T}"/> que contiene los elementos y metadatos de paginación.</returns>
        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> source,
            int pageNumber,
            int pageSize
,
            CancellationToken cancellationToken)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
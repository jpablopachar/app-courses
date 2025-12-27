using Microsoft.EntityFrameworkCore;

namespace Application.Core
{
    /// <summary>
    /// Represents a paged list of items with pagination metadata.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="items">The items in the current page.</param>
    /// <param name="count">The total number of items.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    public class PagedList<T>(
    List<T> items,
    int count,
    int pageNumber,
    int pageSize
    )
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; } = pageNumber;

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize { get; set; } = pageSize;

        /// <summary>
        /// Gets or sets the total number of items.
        /// </summary>
        public int TotalCount { get; set; } = count;

        /// <summary>
        /// Gets or sets the items in the current page.
        /// </summary>
        public List<T> Items { get; set; } = items;

        /// <summary>
        /// Creates a paged list asynchronously from an <see cref="IQueryable{T}"/> source.
        /// </summary>
        /// <param name="source">The queryable data source.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PagedList{T}"/> containing the items and pagination metadata.</returns>
        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> source,
            int pageNumber,
            int pageSize
        )
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
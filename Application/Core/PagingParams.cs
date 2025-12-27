namespace Application.Core
{
    /// <summary>
    /// Represents the base parameters for paging operations, including page number, page size, and sorting options.
    /// </summary>
    public abstract class PagingParams
    {
        /// <summary>
        /// Gets or sets the current page number. Default is 1.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// The maximum allowed page size.
        /// </summary>
        private const int MaxPageSize = 50;
        /// <summary>
        /// The backing field for the page size.
        /// </summary>
        private int _pageSize = 10;
        /// <summary>
        /// Gets or sets the number of items per page. The value cannot exceed <see cref="MaxPageSize"/>.
        /// Default is 10.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        /// <summary>
        /// Gets or sets the property name to order the results by.
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ordering is ascending. Default is true.
        /// </summary>
        public bool? OrderAsc { get; set; } = true;
    }
}
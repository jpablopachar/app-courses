namespace Application.Core
{
    /// <summary>
    /// Representa los parámetros base para operaciones de paginación, incluyendo número de página, tamaño de página y opciones de ordenamiento.
    /// </summary>
    public abstract class PagingParams
    {
        /// <summary>
        /// Obtiene o establece el número de página actual. El valor predeterminado es 1.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// El tamaño máximo permitido de página.
        /// </summary>
        private const int MaxPageSize = 50;
        /// <summary>
        /// Campo de respaldo para el tamaño de página.
        /// </summary>
        private int _pageSize = 10;
        /// <summary>
        /// Obtiene o establece el número de elementos por página. El valor no puede exceder <see cref="MaxPageSize"/>.
        /// El valor predeterminado es 10.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        /// <summary>
        /// Obtiene o establece el nombre de la propiedad por la que se ordenan los resultados.
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el orden es ascendente. El valor predeterminado es true.
        /// </summary>
        public bool? OrderAsc { get; set; } = true;
    }
}
namespace Domain
{
    /// <summary>
    /// Representa los detalles de precio de un curso, incluyendo precios actuales y promocionales.
    /// </summary>
    public class Price : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el nombre del precio.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Obtiene o establece el valor del precio actual.
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Obtiene o establece el valor del precio promocional.
        /// </summary>
        public decimal PromotionalPrice { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de cursos asociada a este precio.
        /// </summary>
        public ICollection<Course>? Courses { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de relaciones curso-precio.
        /// </summary>
        public ICollection<CoursePrice>? CoursePrices { get; set; }
    }
}
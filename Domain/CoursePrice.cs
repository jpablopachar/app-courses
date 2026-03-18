namespace Domain
{
    /// <summary>
    /// Representa la asociación entre un Curso y su Precio.
    /// </summary>
    public class CoursePrice
    {
        /// <summary>
        /// Obtiene o establece el identificador único del Curso asociado.
        /// </summary>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Obtiene o establece la entidad Curso asociada a este CoursePrice.
        /// </summary>
        public Course? Course { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador único del Precio asociado.
        /// </summary>
        public Guid? PriceId { get; set; }

        /// <summary>
        /// Obtiene o establece la entidad Precio asociada a este CoursePrice.
        /// </summary>
        public Price? Price { get; set; }
    }
}
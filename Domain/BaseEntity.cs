namespace Domain
{
    /// <summary>
    /// Representa la clase base para todas las entidades del dominio.
    /// Proporciona un identificador único para cada entidad.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la entidad.
        /// </summary>
        public Guid Id { get; set; }
    }
}

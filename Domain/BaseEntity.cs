namespace Domain
{
    /// <summary>
    /// Represents the base class for all entities in the domain.
    /// Provides a unique identifier for each entity.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }
    }
}

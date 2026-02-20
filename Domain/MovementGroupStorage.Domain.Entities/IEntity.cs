namespace MovementGroupStorage.Domain.Entities
{
    /// <summary>
    /// Defines a domain entity with the key of type <see cref="TKey"/>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey>
    {
        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        TKey Id { get; }
    }
}

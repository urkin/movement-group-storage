namespace MovementGroupStorage.Domain.Entities
{
    /// <summary>
    /// Specifies the custom name of the collection to which this entity type should be mapped.
    /// </summary>
    /// <remarks>
    /// Use this attribute on your entity classes to avoid hardcoded strings.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="CollectionNameAttribute"/> with the given collection name.
        /// </summary>
        /// <param name="collectionName">The name of the collection. May not be null.</param>
        /// <exception cref="ArgumentException">Thrown if the collectionName is null or whitespace.</exception>
        public CollectionNameAttribute(string collectionName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);
            CollectionName = collectionName;
        }

        /// <summary>
        /// Gets the collection name.
        /// </summary>
        /// <value>A string representing the collection name in the database.</value>
        public string CollectionName { get; }
    }
}

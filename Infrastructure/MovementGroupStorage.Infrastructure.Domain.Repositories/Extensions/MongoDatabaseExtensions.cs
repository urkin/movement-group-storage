using MongoDB.Driver;
using MovementGroupStorage.Domain.Entities;
using System.Text.RegularExpressions;

namespace MovementGroupStorage.Infrastructure.Domain.Repositories.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IMongoDatabase"/> interface to simplify collection retrieval.
    /// </summary>
    public static class MongoDatabaseExtensions
    {
        /// <summary>
        /// Gets a MongoDB collection using the name specified in the <see cref="CollectionNameAttribute"/> on the entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type representing the documents in the collection.</typeparam>
        /// <param name="database">The <see cref="IMongoDatabase"/> instance.</param>
        /// <param name="settings">Optional <see cref="MongoCollectionSettings"/> for the collection.</param>
        /// <returns>An <see cref="IMongoCollection{TEntity}"/> instance.</returns>
        /// <remarks>
        /// If the <see cref="CollectionNameAttribute"/> is not present, it defaults to the class name.
        /// </remarks>
        public static IMongoCollection<TEntity> GetCollection<TEntity>(this IMongoDatabase database, MongoCollectionSettings? settings = null)
        {
            var entityType = typeof(TEntity);
            var attribute = entityType
                .GetCustomAttributes(typeof(CollectionNameAttribute), true)
                .FirstOrDefault() as CollectionNameAttribute;
            var collection = attribute != null
                ? attribute.CollectionName
                : Regex.Replace(entityType.Name, "mongo(db)?", string.Empty, RegexOptions.IgnoreCase);

            return database.GetCollection<TEntity>(collection, settings);
        }
    }
}

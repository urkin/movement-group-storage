using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovementGroupStorage.Domain.Entities
{
    /// <summary>
    /// Represents basic MongoDB entity.
    /// </summary>
    public abstract class MongoEntity : IEntity<string>
    {
        /// <inheritdoc/>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; } = default!;
    }
}

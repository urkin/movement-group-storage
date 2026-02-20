using MongoDB.Bson.Serialization.Attributes;

namespace MovementGroupStorage.Domain.Entities
{
    /// <summary>
    /// Represents a document of the dump collection.
    /// </summary>
    [CollectionName("dump")]
    public class MongoDump : MongoEntity
    {
        /// <summary>
        /// Represents a value of unknown type.
        /// </summary>
        [BsonElement("value")]
        public object Value { get; set; } = default!;
    }
}

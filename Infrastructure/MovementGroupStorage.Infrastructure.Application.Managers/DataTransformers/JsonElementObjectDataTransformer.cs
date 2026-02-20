using MongoDB.Bson.Serialization;
using MovementGroupStorage.Application.Services;
using System.Text.Json;

namespace MovementGroupStorage.Infrastructure.Application.Managers
{
    /// <summary>
    /// Service that transforms data from <see cref="JsonElement"/> to <see cref="object"/>.
    /// </summary>
    public class JsonElementObjectDataTransformer : IDataTransformer<JsonElement, object>
    {
        /// <inheritdoc/>
        public object Transform(JsonElement source)
        {
            return source.ValueKind switch
            {
                JsonValueKind.String => source.GetString()!,
                JsonValueKind.Number => source.TryGetInt64(out long l) ? l : source.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null!,
                _ => BsonSerializer.Deserialize<object>(source.GetRawText())
            };
        }

        /// <inheritdoc/>
        public Task<object> TransformAsync(JsonElement source)
        {
            var destination = Transform(source);
            return Task.FromResult(destination);
        }
    }
}

namespace MovementGroupStorage.Application.Models
{
    /// <summary>
    /// Represents identifier application model.
    /// </summary>
    public class Identifier<TId>
    {
        public Identifier(TId id)
        {
            Id = id;
        }

        /// <summary>
        /// Id of type <see cref="TId"/>
        /// </summary>
        public TId Id { get; }
    }
}

namespace MovementGroupStorage.Application.Models
{
    /// <summary>
    /// Represents options of a MongoDB connection.
    /// </summary>
    public class MongoOptions
    {
        /// <summary>
        /// Gets or sets databasename.
        /// </summary>
        public string Database { get; set; } = default!;

        /// <summary>
        /// Gets or sets MongoDB connection string.
        /// </summary>
        public string ConnectionString { get; set; } = default!;
    }
}

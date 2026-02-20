namespace MovementGroupStorage.Application.Models
{
    /// <summary>
    /// Represents options of a distributed service.
    /// </summary>
    public class DistributedCacheServiceOptions
    {
        /// <summary>
        /// Gets expiration seconds amount.
        /// </summary>
        public int ExpirationSeconds { get; set; }

        /// <summary>
        /// Gets expiration type.
        /// </summary>
        public ExpirationType ExpirationType { get; set; }
    }
}

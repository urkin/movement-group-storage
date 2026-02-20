namespace MovementGroupStorage.Application.Models
{
    /// <summary>
    /// Represents possible values of Expiration Type.
    /// </summary>
    public enum ExpirationType
    {
        /// <summary>
        /// Unknown expiration type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Absolute Expiration.
        /// </summary>
        Absolute = 1,

        /// <summary>
        /// How long a cache entry can be inactive (e.g. not accessed)
        /// </summary>
        Sliding = 2,
    }
}

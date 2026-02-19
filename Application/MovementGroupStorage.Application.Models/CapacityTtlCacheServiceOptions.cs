namespace MovementGroupStorage.Application.Models
{
    /// <summary>
    /// Represents options of a cache service with capacity TTL.
    /// </summary>
    public class CapacityTtlCacheServiceOptions
    {
        /// <summary>
        /// The service capacity.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Valid range of the capacity value.
        /// </summary>
        public Range<int> CapacityRange { get; set; }
    }
}

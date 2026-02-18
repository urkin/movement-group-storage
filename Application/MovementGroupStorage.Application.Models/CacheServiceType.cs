namespace MovementGroupStorage.Application.Models
{
    public enum CacheServiceType
    {
        /// <summary>
        /// Unknown type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Distributed cache service.
        /// </summary>
        Distributed = 1,

        /// <summary>
        /// Memory cache service.
        /// </summary>
        Memory = 2,
    }
}

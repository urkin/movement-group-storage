namespace MovementGroupStorage.Application.Models
{
    /// <summary>
    /// Represents possible values of Application Service Result Status
    /// </summary>
    public enum ApplicationServiceResultStatus
    {
        /// <summary>
        /// Unknown status.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Application Service succeeded.
        /// </summary>
        Succeeded = 1,

        /// <summary>
        /// Data created.
        /// </summary>
        Created = 2,

        /// <summary>
        /// Application Service failed.
        /// </summary>
        Failed = 3,

        /// <summary>
        /// Invalid input parameter(s).
        /// </summary>
        InvalidInput = 4,

        /// <summary>
        /// Data already exists.
        /// </summary>
        AlreadyExists = 5,

        /// <summary>
        /// Data not found.
        /// </summary>
        DoesNotExist = 6,
    }
}

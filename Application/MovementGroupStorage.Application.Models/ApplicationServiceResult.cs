namespace MovementGroupStorage.Application.Models
{
    /// <summary>
    /// Represents application service or manager result.
    /// </summary>
    public class ApplicationServiceResult
    {
        public ApplicationServiceResult(ApplicationServiceResultStatus status)
            : this(null, status)
        {
        }

        public ApplicationServiceResult(object? data, ApplicationServiceResultStatus status)
        {
            Data = data;
            Status = status;
        }

        /// <summary>
        /// Result data of the application service.
        /// </summary>
        public object? Data { get; }

        /// <summary>
        /// Result status.
        /// </summary>
        public ApplicationServiceResultStatus Status { get; }
    }
}

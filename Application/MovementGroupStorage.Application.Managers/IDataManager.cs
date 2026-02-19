using MovementGroupStorage.Application.Models;

namespace MovementGroupStorage.Application.Managers
{
    /// <summary>
    /// Defines Application Layer Manager encapsulating manage Data actions.
    /// </summary>
    public interface IDataManager
    {
        /// <summary>
        /// Fetches data from a data storage.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>An asynchronous operation that returns <see cref="ApplicationServiceResult"/>.</returns>
        Task<ApplicationServiceResult> FetchAsync(string key);

        /// <summary>
        /// Saves data into a data storage.
        /// </summary>
        /// <param name="data">Data to save</param>
        /// <returns>An asynchronous operation that returns <see cref="ApplicationServiceResult"/>.</returns>
        Task<ApplicationServiceResult> CreateAsync(dynamic data);
    }
}

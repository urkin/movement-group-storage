
using MovementGroupStorage.Application.Models;

namespace MovementGroupStorage.Application.Services
{
    /// <summary>
    /// Defines a cache service with the 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface ICacheService<TKey> where TKey : notnull
    {
        /// <summary>
        /// Gets the count of items within the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the Type of the service.
        /// </summary>
        CacheServiceType Type { get; }

        /// <summary>
        /// Removes the value associated with the given key.
        /// </summary>
        /// <param name="key">An key identifying the entry.</param>
        /// <returns>An asynchronous operation that returns <see cref="ApplicationServiceResult"/>.</returns>
        Task<ApplicationServiceResult> RemoveAsync(TKey key);

        /// <summary>
        /// Associate a value with a key in the Cache.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to set.</typeparam>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value to associate with the key.</param>
        /// <returns>An asynchronous operation that returns <see cref="ApplicationServiceResult"/>.</returns>
        Task<ApplicationServiceResult> SetAsync<TValue>(TKey key, TValue value);

        /// <summary>
        /// Try to get the value associated with the given key.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to get.</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>An asynchronous operation that returns <see cref="ApplicationServiceResult"/>.</returns>
        Task<ApplicationServiceResult> GetAsync<TValue>(TKey key);
    }
}
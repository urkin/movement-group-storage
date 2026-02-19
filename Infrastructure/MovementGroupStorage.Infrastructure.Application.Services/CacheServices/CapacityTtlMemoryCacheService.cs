using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;
using System.Collections.Concurrent;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// A memory cache service with capacity TTL.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class CapacityTtlMemoryCacheService<TKey> : ICacheService<TKey> where TKey : notnull
    {
        private readonly int _capacity;
        private readonly IMemoryCache _cache;
        private readonly SemaphoreSlim _mutex;
        private readonly ConcurrentDictionary<TKey, long> _timestamps;

        /// <summary>
        /// Instantiate a <see cref="CapacityTtlMemoryCacheService"/> instance.
        /// </summary>
        /// <param name="cache">An instance of <see cref="IMemoryCache"/></param>
        /// <param name="options">An instance of <see cref="CapacityTtlCacheServiceOptions"/></param>
        public CapacityTtlMemoryCacheService(
            IMemoryCache cache,
            IOptions<CapacityTtlCacheServiceOptions> options)
        {
            ArgumentNullException.ThrowIfNull(cache);
            ArgumentNullException.ThrowIfNull(options);

            _cache = cache;
            _mutex = new(1, 1);
            _timestamps = new();
            _capacity = options.Value.Capacity;
        }

        /// <inheritdoc/>
        public int Count => _timestamps.Count;

        /// <inheritdoc/>
        public CacheServiceType Type => CacheServiceType.Memory;

        /// <inheritdoc/>
        public async Task<ApplicationServiceResult> SetAsync<TValue>(TKey key, TValue value)
        {
            await _mutex.WaitAsync();
            try
            {
                if (!_timestamps.ContainsKey(key))
                {
                    RemoveExtra();
                }

                _cache.Set(key, value);
                _timestamps[key] = CurrentTimestamp;

                return new ApplicationServiceResult(ApplicationServiceResultStatus.Succeeded);
            }
            finally
            {
                _mutex.Release();
            }
        }

        /// <inheritdoc/>
        public Task<ApplicationServiceResult> GetAsync<TValue>(TKey key)
        {
            if (_cache.TryGetValue(key, out TValue? value))
            {
                // update last access timestamp
                _timestamps[key] = CurrentTimestamp;

                return Task.FromResult(
                    new ApplicationServiceResult(value, ApplicationServiceResultStatus.Succeeded));
            }
            else
            {
                return Task.FromResult(
                    new ApplicationServiceResult(ApplicationServiceResultStatus.DoesNotExist));
            }
        }

        /// <inheritdoc/>
        public async Task<ApplicationServiceResult> RemoveAsync(TKey key)
        {
            await _mutex.WaitAsync();
            try
            {
                var removed = _timestamps.TryRemove(key, out _);
                if (removed)
                {
                    _cache.Remove(key);
                }

                return new ApplicationServiceResult(ApplicationServiceResultStatus.Succeeded);
            }
            finally
            {
                _mutex.Release();
            }
        }

        private static long CurrentTimestamp =>
            DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        private void RemoveExtra()
        {
            while (_timestamps.Count >= _capacity)
            {
                var oldest = _timestamps
                    .OrderBy(timestamp => timestamp.Value)
                    .FirstOrDefault();

                if (oldest.Key is null)
                    return;

                var removed = _timestamps.TryRemove(oldest.Key, out _);
                if (removed)
                {
                    _cache.Remove(oldest.Key);
                }
            }
        }
    }
}

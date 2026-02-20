using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// A distributed cache service using Redis.
    /// </summary>
    public class RedisDistributedCacheService : ICacheService<string>
    {
        private readonly IDatabase _database;
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;

        public RedisDistributedCacheService(
            IDistributedCache cache,
            IConnectionMultiplexer redis,
            IOptions<DistributedCacheEntryOptions> options)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _database = redis?.GetDatabase() ?? throw new ArgumentNullException(nameof(redis));
        }

        /// <inheritdoc/>
        public int Count => (int?)_database.Execute("DBSIZE") ?? 0;

        /// <inheritdoc/>
        public CacheServiceType Type => CacheServiceType.Distributed;

        /// <inheritdoc/>
        public async Task<ApplicationServiceResult> RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
            return new ApplicationServiceResult(ApplicationServiceResultStatus.Succeeded);
        }

        /// <inheritdoc/>
        public async Task<ApplicationServiceResult> SetAsync<TValue>(string key, TValue value)
        {
            if (value == null || string.IsNullOrWhiteSpace(key))
            {
                return new ApplicationServiceResult(ApplicationServiceResultStatus.InvalidInput);
            }

            var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
            await _cache.SetAsync(key, bytes, _options);
            return new ApplicationServiceResult(ApplicationServiceResultStatus.Succeeded);
        }

        /// <inheritdoc/>
        public async Task<ApplicationServiceResult> GetAsync<TValue>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new ApplicationServiceResult(ApplicationServiceResultStatus.InvalidInput);
            }

            var bytes = await _cache.GetAsync(key);
            if (bytes == null)
            {
                return new ApplicationServiceResult(ApplicationServiceResultStatus.DoesNotExist);
            }

            var value = JsonSerializer.Deserialize<TValue>(bytes);
            return new ApplicationServiceResult(value, ApplicationServiceResultStatus.Succeeded);
        }
    }
}

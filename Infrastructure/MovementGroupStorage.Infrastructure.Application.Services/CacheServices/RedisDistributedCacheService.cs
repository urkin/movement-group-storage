using Microsoft.Extensions.Caching.Distributed;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    public class RedisDistributedCacheService : ICacheService<string>
    {
        private readonly IDatabase _database;
        private readonly IDistributedCache _cache;

        public RedisDistributedCacheService(IDistributedCache cache, IConnectionMultiplexer redis)
        {
            ArgumentNullException.ThrowIfNull(redis);

            _database = redis.GetDatabase();
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
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
            await _cache.SetAsync(key, bytes);
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
            var value = JsonSerializer.Deserialize<TValue>(bytes);
            return new ApplicationServiceResult(value, ApplicationServiceResultStatus.Succeeded);
        }
    }
}

using Microsoft.Extensions.Caching.Distributed;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service that transforms <see cref="DistributedCacheEntryOptions.AbsoluteExpirationRelativeToNow"/>
    /// from <see cref="DistributedCacheServiceOptions"/> to <see cref="DistributedCacheEntryOptions"/>.
    /// </summary>
    public class DistributedCacheEntryOptionsAbsoluteExpirationDataTransformer : IDataTransformer<DistributedCacheServiceOptions, DistributedCacheEntryOptions>
    {
        /// <inheritdoc/>
        public DistributedCacheEntryOptions Transform(DistributedCacheServiceOptions source)
        {
            var destianation = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(source.ExpirationSeconds),
            };

            return destianation;
        }

        /// <inheritdoc/>
        public Task<DistributedCacheEntryOptions> TransformAsync(DistributedCacheServiceOptions source)
        {
            var destianation = Transform(source);
            return Task.FromResult(destianation);
        }
    }
}

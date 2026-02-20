using Microsoft.Extensions.Caching.Distributed;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service that transforms <see cref="DistributedCacheEntryOptions.SlidingExpiration"/>
    /// from <see cref="DistributedCacheServiceOptions"/> to <see cref="DistributedCacheEntryOptions"/>.
    /// </summary>
    public class DistributedCacheEntryOptionsSlidingExpirationDataTransformer : IDataTransformer<DistributedCacheServiceOptions, DistributedCacheEntryOptions>
    {
        /// <inheritdoc/>
        public DistributedCacheEntryOptions Transform(DistributedCacheServiceOptions source)
        {
            var destianation = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(source.ExpirationSeconds),
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

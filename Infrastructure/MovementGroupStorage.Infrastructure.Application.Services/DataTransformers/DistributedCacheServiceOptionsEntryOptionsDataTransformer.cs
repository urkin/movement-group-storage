using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service that transforms data from <see cref="DistributedCacheServiceOptions"/>
    /// to <see cref="DistributedCacheEntryOptions"/>.
    /// </summary>
    public class DistributedCacheServiceOptionsEntryOptionsDataTransformer : IDataTransformer<DistributedCacheServiceOptions, DistributedCacheEntryOptions>
    {
        private readonly IServiceProvider _serviceProvider;

        public DistributedCacheServiceOptionsEntryOptionsDataTransformer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc/>
        public DistributedCacheEntryOptions Transform(DistributedCacheServiceOptions source)
        {
            var transformer = _serviceProvider
                .GetRequiredKeyedService<IDataTransformer<DistributedCacheServiceOptions, DistributedCacheEntryOptions>>(source.ExpirationType);
            return transformer.Transform(source);
        }

        /// <inheritdoc/>
        public async Task<DistributedCacheEntryOptions> TransformAsync(DistributedCacheServiceOptions source)
        {
            var transformer = _serviceProvider
                .GetRequiredKeyedService<IDataTransformer<DistributedCacheServiceOptions, DistributedCacheEntryOptions>>(source.ExpirationType);
            var destination = await transformer.TransformAsync(source);
            return destination;
        }
    }
}

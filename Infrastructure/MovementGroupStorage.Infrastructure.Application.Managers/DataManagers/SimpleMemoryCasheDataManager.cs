using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;

namespace MovementGroupStorage.Infrastructure.Application.Managers
{
    /// <summary>
    /// Manages data in memory cache.
    /// </summary>
    public class SimpleMemoryCasheDataManager : SimpleCasheDataManagerBase
    {
        public SimpleMemoryCasheDataManager(
            ILogger<SimpleMemoryCasheDataManager> logger,
            [FromKeyedServices(CacheServiceType.Memory)] ICacheService<string> cacheService)
            : base (logger, cacheService)
        {
        }
    }
}

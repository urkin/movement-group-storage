using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;

namespace MovementGroupStorage.Infrastructure.Application.Managers
{
    /// <summary>
    /// Manages data in distributed cache.
    /// </summary>
    public class SimpleDistributedCasheDataManager : SimpleCasheDataManagerBase
    {
        public SimpleDistributedCasheDataManager(
            ILogger<SimpleDistributedCasheDataManager> logger,
            [FromKeyedServices(CacheServiceType.Distributed)] ICacheService<string> cacheService)
            : base (logger, cacheService)
        {
        }
    }
}

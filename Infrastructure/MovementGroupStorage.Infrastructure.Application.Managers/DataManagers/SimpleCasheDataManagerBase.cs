using Microsoft.Extensions.Logging;
using MovementGroupStorage.Application.Managers;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;

namespace MovementGroupStorage.Infrastructure.Application.Managers
{
    /// <summary>
    /// Manages data using provided cache service.
    /// </summary>
    public abstract class SimpleCasheDataManagerBase : IDataManager
    {
        protected readonly ILogger _logger;
        protected readonly ICacheService<string> _cacheService;

        protected SimpleCasheDataManagerBase(
            ILogger logger,
            ICacheService<string> cacheService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        /// <inheritdoc/>
        public virtual async Task<ApplicationServiceResult> FetchAsync(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return new ApplicationServiceResult(ApplicationServiceResultStatus.InvalidInput);
                }

                var result = await _cacheService.GetAsync<dynamic>(key);
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogDebug(exception, $"FAILED fetching the data by key `{key}`");
                return new ApplicationServiceResult(ApplicationServiceResultStatus.Failed);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<ApplicationServiceResult> CreateAsync(dynamic data)
        {
            try
            {
                var key = Guid.NewGuid().ToString("N");
                await _cacheService.SetAsync(key, data);

                return new ApplicationServiceResult(
                    new Identifier<string>(key), ApplicationServiceResultStatus.Created);
            }
            catch (Exception exception)
            {
                _logger.LogDebug(exception, $"FAILED saving the data `{data}`");
                return new ApplicationServiceResult(ApplicationServiceResultStatus.Failed);
            }
        }
    }
}

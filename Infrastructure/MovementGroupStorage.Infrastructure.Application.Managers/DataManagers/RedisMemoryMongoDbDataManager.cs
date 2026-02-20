using Microsoft.Extensions.Logging;
using MovementGroupStorage.Application.Managers;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;
using MovementGroupStorage.Domain.Entities;
using MovementGroupStorage.Domain.Repositories;
using System.Text.Json;

namespace MovementGroupStorage.Infrastructure.Application.Managers
{
    /// <summary>
    /// Manages data using MongoDB.
    /// </summary>
    public class RedisMemoryMongoDbDataManager : IDataManager
    {
        private readonly ILogger _logger;
        private readonly IRepository<string, MongoDump> _repository;
        private readonly IEnumerable<ICacheService<string>> _cacheServices;
        private readonly IDataTransformer<JsonElement, object> _dataTransformer;

        public RedisMemoryMongoDbDataManager(
            IRepository<string, MongoDump> repository,
            ILogger<RedisMemoryMongoDbDataManager> logger,
            IEnumerable<ICacheService<string>> cacheServices,
            IDataTransformer<JsonElement, object> dataTransformer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _cacheServices = cacheServices ?? throw new ArgumentNullException(nameof(cacheServices));
            _dataTransformer = dataTransformer ?? throw new ArgumentNullException(nameof(dataTransformer));
        }

        /// <inheritdoc/>
        public virtual async Task<ApplicationServiceResult> FetchAsync(string key)
        {
            try
            {
                // Validate input data
                if (string.IsNullOrEmpty(key))
                {
                    return new ApplicationServiceResult(ApplicationServiceResultStatus.InvalidInput);
                }

                // Try to fetch the data from one of the caches (Redis first).
                var data = await ResolveData(key);
                if (data != null)
                {
                    return new ApplicationServiceResult(data, ApplicationServiceResultStatus.Succeeded);
                }

                // If the data was not found fetch it from the DB.
                var document = await _repository.FindAsync(key);
                if (document == null)
                {
                    return new ApplicationServiceResult(ApplicationServiceResultStatus.DoesNotExist);
                }

                // If the data was found save it to all the caches.
                await Task.WhenAll(_cacheServices.Select(service => service.SetAsync(key, document.Value)));

                // ...and return it to a client.
                return new ApplicationServiceResult(document.Value, ApplicationServiceResultStatus.Succeeded);
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
                var document = await _repository.InsertAsync(new MongoDump
                {
                    Value = data is JsonElement element ? _dataTransformer.Transform(element) : data,
                });

                return new ApplicationServiceResult(
                    new Identifier<string>(document.Id), ApplicationServiceResultStatus.Created);
            }
            catch (Exception exception)
            {
                _logger.LogDebug(exception, $"FAILED inserting the data `{data}`");
                return new ApplicationServiceResult(ApplicationServiceResultStatus.Failed);
            }
        }

        private async Task<dynamic?> ResolveData(string key)
        {
            var data = await ResolveServiceData(
                key, _cacheServices.FirstOrDefault(service => service.Type == CacheServiceType.Distributed));
            if (data == null)
            {
                data = await ResolveServiceData(
                    key, _cacheServices.FirstOrDefault(service => service.Type == CacheServiceType.Memory));
            }

            return data;
        }

        private async Task<dynamic?> ResolveServiceData(string key, ICacheService<string>? cacheService)
        {
            if (cacheService != null)
            {
                var result = await cacheService.GetAsync<dynamic>(key);
                if (result.Status == ApplicationServiceResultStatus.Succeeded)
                {
                    return result.Data;
                }
            }

            return null;
        }
    }
}

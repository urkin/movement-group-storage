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
    public class MongoDbDataManager : IDataManager
    {
        private readonly ILogger _logger;
        private readonly IRepository<string, MongoDump> _repository;
        private readonly IDataTransformer<JsonElement, object> _dataTransformer;

        public MongoDbDataManager(
            ILogger<MongoDbDataManager> logger,
            IRepository<string, MongoDump> repository,
            IDataTransformer<JsonElement, object> dataTransformer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _dataTransformer = dataTransformer ?? throw new ArgumentNullException(nameof(dataTransformer));
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

                var document = await _repository.FindAsync(key);
                if (document == null)
                {
                    return new ApplicationServiceResult(ApplicationServiceResultStatus.DoesNotExist);
                }

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
                _logger.LogDebug(exception, $"FAILED saving the data `{data}`");
                return new ApplicationServiceResult(ApplicationServiceResultStatus.Failed);
            }
        }
    }
}

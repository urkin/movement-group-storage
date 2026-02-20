using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Managers;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;
using MovementGroupStorage.Infrastructure.Application.Managers;
using MovementGroupStorage.Infrastructure.Application.Services;
using StackExchange.Redis;
using System.Net;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCacheServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            var redisConection = configuration
                .GetSection("Redis")
                .GetValue<string>("ConnectionString")!;
            var multiplexer = ConnectionMultiplexer.Connect(redisConection);

            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "MovementGroupStorage:";
                options.ConnectionMultiplexerFactory =
                    () => Task.FromResult<IConnectionMultiplexer>(multiplexer);
            });

            services.AddOptions<CapacityTtlCacheServiceOptions>()
                .BindConfiguration("CapacityTtlCacheServiceOptions")
                .ValidateOnStart();

            services.AddOptions<DistributedCacheServiceOptions>()
                .BindConfiguration("DistributedCacheServiceOptions")
                .ValidateOnStart();

            // Use Chain of Responsibility Design Pattern to validate `CapacityTtlCacheServiceOptions`.
            // Learn more about Strategy Design Pattern: https://refactoring.guru/design-patterns/chain-of-responsibility/csharp/example
            services
                .AddSingleton<IValidateOptions<CapacityTtlCacheServiceOptions>, CapacityTtlCacheServiceOptionsCapacityRangeHasValueValidator>()
                .AddSingleton<IValidateOptions<CapacityTtlCacheServiceOptions>, CapacityTtlCacheServiceOptionsCapacityRangeMaxGreaterThanZeroValidator>()
                .AddSingleton<IValidateOptions<CapacityTtlCacheServiceOptions>, CapacityTtlCacheServiceOptionsCapacityRangeMinGreaterThanZeroValidator>()
                .AddSingleton<IValidateOptions<CapacityTtlCacheServiceOptions>, CapacityTtlCacheServiceOptionsCapacityRangeCorrectValidator>()
                .AddSingleton<IValidateOptions<CapacityTtlCacheServiceOptions>, CapacityTtlCacheServiceOptionsCapacityGreaterThanZeroValidator>()
                .AddSingleton<IValidateOptions<CapacityTtlCacheServiceOptions>, CapacityTtlCacheServiceOptionsCapacityMinValueValidator>()
                .AddSingleton<IValidateOptions<CapacityTtlCacheServiceOptions>, CapacityTtlCacheServiceOptionsCapacityMaxValueValidator>();

            // Use Chain of Responsibility Design Pattern to validate `DistributedCacheServiceOptions`.
            // Learn more about Strategy Design Pattern: https://refactoring.guru/design-patterns/chain-of-responsibility/csharp/example
            services
                .AddSingleton<IValidateOptions<DistributedCacheServiceOptions>, DistributedCacheServiceOptionsKnownExpirationTypeValidator>()
                .AddSingleton<IValidateOptions<DistributedCacheServiceOptions>, DistributedCacheServiceOptionsExpirationSecondsGreaterThanZeroValidator>();

            services.AddSingleton(
                typeof(ICacheService<>),
                typeof(CapacityTtlMemoryCacheService<>));

            services.AddKeyedSingleton(
                typeof(ICacheService<>),
                CacheServiceType.Memory,
                typeof(CapacityTtlMemoryCacheService<>));

            services
                .AddSingleton<IDataTransformer<DistributedCacheServiceOptions, DistributedCacheEntryOptions>, DistributedCacheServiceOptionsEntryOptionsDataTransformer>()
                .AddKeyedSingleton<IDataTransformer<DistributedCacheServiceOptions, DistributedCacheEntryOptions>, DistributedCacheEntryOptionsSlidingExpirationDataTransformer>(ExpirationType.Sliding)
                .AddKeyedSingleton<IDataTransformer<DistributedCacheServiceOptions, DistributedCacheEntryOptions>, DistributedCacheEntryOptionsAbsoluteExpirationDataTransformer>(ExpirationType.Absolute);

            services.AddSingleton<IOptions<DistributedCacheEntryOptions>>(serviceProvider =>
            {
                var options = serviceProvider
                    .GetRequiredService<IOptions<DistributedCacheServiceOptions>>();
                var transformer = serviceProvider
                    .GetRequiredService<IDataTransformer<DistributedCacheServiceOptions, DistributedCacheEntryOptions>>();

                return Options.Options.Create(transformer.Transform(options.Value));
            });

            services.AddSingleton<ICacheService<string>, RedisDistributedCacheService>();
            services.AddKeyedSingleton<ICacheService<string>, RedisDistributedCacheService>(CacheServiceType.Distributed);

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var resolveBadRequest = () => HttpStatusCode.BadRequest;

            services.AddScoped<IDataManager, SimpleDistributedCasheDataManager>();

            // Register HTTP Status Code resolvers
            // according to Strategy Design Pattern to resolve them in runtime.
            // Learn more about Strategy Design Pattern: https://refactoring.guru/design-patterns/strategy/csharp/example
            services
                .AddKeyedSingleton<Func<HttpStatusCode>>(ApplicationServiceResultStatus.InvalidInput, resolveBadRequest);
            services
                .AddKeyedSingleton<Func<HttpStatusCode>>(ApplicationServiceResultStatus.AlreadyExists, resolveBadRequest);
            services
                .AddKeyedSingleton<Func<HttpStatusCode>>(ApplicationServiceResultStatus.Succeeded, () => HttpStatusCode.OK);
            services
                .AddKeyedSingleton<Func<HttpStatusCode>>(ApplicationServiceResultStatus.Created, () => HttpStatusCode.Created);
            services
                .AddKeyedSingleton<Func<HttpStatusCode>>(ApplicationServiceResultStatus.DoesNotExist, () => HttpStatusCode.NotFound);
            services
                .AddKeyedSingleton<Func<HttpStatusCode>>(ApplicationServiceResultStatus.Unknown, () => HttpStatusCode.NotImplemented);
            services
                .AddKeyedSingleton<Func<HttpStatusCode>>(ApplicationServiceResultStatus.Failed, () => HttpStatusCode.UnprocessableEntity);

            return services;
        }
    }
}

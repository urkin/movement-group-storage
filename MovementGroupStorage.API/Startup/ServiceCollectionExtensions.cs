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

            services.AddSingleton(
                typeof(ICacheService<>),
                typeof(CapacityTtlMemoryCacheService<>));

            services.AddKeyedSingleton(
                typeof(ICacheService<>),
                CacheServiceType.Memory,
                typeof(CapacityTtlMemoryCacheService<>));

            services.AddSingleton<ICacheService<string>, RedisDistributedCacheService>();
            services.AddKeyedSingleton<ICacheService<string>, RedisDistributedCacheService>(CacheServiceType.Distributed);

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var resolveBadRequest = () => HttpStatusCode.BadRequest;

            services.AddScoped<IDataManager, SimpleMemoryCasheDataManager>();

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

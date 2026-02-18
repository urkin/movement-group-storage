using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Application.Services;
using MovementGroupStorage.Infrastructure.Application.Services;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCacheServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.AddSingleton(
                typeof(ICacheService<>),
                typeof(CapacityTtlMemoryCacheService<>));

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

            services.AddSingleton<ICacheService<string>, RedisDistributedCacheService>();

            services.AddOptions<CapacityTtlCacheServiceOptions>()
                .BindConfiguration("CapacityTtlCacheServiceOptions")
                .ValidateOnStart();

            return services;
        }
    }
}

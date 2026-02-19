using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Infrastructure.Application.Services;

namespace MovementGroupStorage.Application.UnitTests
{
    public class CapacityTtlMemoryCacheServiceTests
    {
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        [Fact]
        public async Task Set_And_Get_Should_Work()
        {
            var options = Options.Create(new CapacityTtlCacheServiceOptions
            {
                Capacity = 3
            });
            var target = new CapacityTtlMemoryCacheService<string>(_cache, options);

            await target.SetAsync("a", 10);

            var result = await target.GetAsync<int>("a");

            result.Should().NotBeNull();
            result.Status.Should().Be(ApplicationServiceResultStatus.Succeeded);
            result.Data.Should().Be(10);
        }

        [Fact]
        public async Task Remove_Should_Delete_Item()
        {
            var options = Options.Create(new CapacityTtlCacheServiceOptions
            {
                Capacity = 3
            });
            var target = new CapacityTtlMemoryCacheService<string>(_cache, options);

            await target.SetAsync("a", 1);
            var removeResult = await target.RemoveAsync("a");
            removeResult.Should().NotBeNull();
            removeResult.Status.Should().Be(ApplicationServiceResultStatus.Succeeded);

            var getResult = await target.GetAsync<int>("a");
            getResult.Should().NotBeNull();
            getResult.Status.Should().Be(ApplicationServiceResultStatus.DoesNotExist);
        }

        [Fact]
        public async Task Capacity_Should_Evict_Oldest()
        {
            var a = 1;
            var b = 2;
            var c = 3;
            var options = Options.Create(new CapacityTtlCacheServiceOptions
            {
                Capacity = 2
            });
            var target = new CapacityTtlMemoryCacheService<string>(_cache, options);

            await target.SetAsync("a", a);
            await Task.Delay(1000);
            await target.SetAsync("b", b);
            await Task.Delay(1000);
            await target.SetAsync("c", c);

            var resultA = await target.GetAsync<int>("a");
            resultA.Should().NotBeNull();
            resultA.Status.Should().Be(ApplicationServiceResultStatus.DoesNotExist);
            resultA.Data.Should().NotBe(a);

            var resultB = await target.GetAsync<int>("b");
            resultB.Should().NotBeNull();
            resultB.Status.Should().Be(ApplicationServiceResultStatus.Succeeded);
            resultB.Data.Should().Be(b);

            var resultC = await target.GetAsync<int>("c");

            resultC.Should().NotBeNull();
            resultC.Status.Should().Be(ApplicationServiceResultStatus.Succeeded);
            resultC.Data.Should().Be(c);
        }

        [Fact]
        public async Task Concurrent_Reads_And_Writes_Should_Not_Throw()
        {
            var options = Options.Create(new CapacityTtlCacheServiceOptions
            {
                Capacity = 50
            });
            var target = new CapacityTtlMemoryCacheService<int>(_cache, options);

            var writers = Enumerable.Range(0, 200)
                .Select(i => Task.Run(async () =>
                {
                    await target.SetAsync(i, i);
                }));

            var readers = Enumerable.Range(0, 200)
                .Select(i => Task.Run(async () =>
                {
                    await target.GetAsync<int>(i);
                }));

            await Task.WhenAll(writers.Concat(readers));

            target.Count.Should().BeLessThanOrEqualTo(50);
        }

        [Fact]
        public async Task Overwrite_Should_Not_Increase_Count()
        {
            var options = Options.Create(new CapacityTtlCacheServiceOptions
            {
                Capacity = 2
            });
            var target = new CapacityTtlMemoryCacheService<string>(_cache, options);

            await target.SetAsync("a", 1);
            await target.SetAsync("a", 2);

            target.Count.Should().Be(1);

            var getResult = await target.GetAsync<int>("a");
            getResult.Should().NotBeNull();
            getResult.Status.Should().Be(ApplicationServiceResultStatus.Succeeded);
            getResult.Data.Should().Be(2);
        }

        [Fact]
        public async Task Concurrent_Writes_Should_Not_Exceed_Capacity()
        {
            var options = Options.Create(new CapacityTtlCacheServiceOptions
            {
                Capacity = 10
            });
            var target = new CapacityTtlMemoryCacheService<int>(_cache, options);

            var tasks = Enumerable.Range(0, 100)
                .Select(i => target.SetAsync(i, i));

            await Task.WhenAll(tasks);

            target.Count.Should().BeLessThanOrEqualTo(10);
        }
    }
}

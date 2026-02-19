using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Infrastructure.Application.Services;

namespace MovementGroupStorage.Application.UnitTests
{
    public class CapacityTtlCacheServiceOptionsCapacityMaxValueValidatorTests
    {
        private readonly CapacityTtlCacheServiceOptionsCapacityMaxValueValidator target;

        public CapacityTtlCacheServiceOptionsCapacityMaxValueValidatorTests()
        {
            target = new CapacityTtlCacheServiceOptionsCapacityMaxValueValidator();
        }

        [Fact]
        public void Validate_ShouldFail_WhenCapacityGreaterThanMax()
        {
            var options = new CapacityTtlCacheServiceOptions
            {
                Capacity = 100,
                CapacityRange = new Range<int>
                {
                    Min = 5,
                    Max = 20
                }
            };

            var result = target.Validate(null, options);

            Assert.False(result.Succeeded);
            Assert.Contains("less than or equal", result.FailureMessage);
        }

        [Fact]
        public void Validate_ShouldSucceed_WhenOptionsAreValid()
        {
            var options = new CapacityTtlCacheServiceOptions
            {
                Capacity = 10,
                CapacityRange = new Range<int>
                {
                    Min = 5,
                    Max = 20
                }
            };

            var result = target.Validate(null, options);

            Assert.True(result.Succeeded);
        }
    }
}

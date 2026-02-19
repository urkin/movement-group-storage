using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Infrastructure.Application.Services;

namespace MovementGroupStorage.Application.UnitTests
{
    public class CapacityTtlCacheServiceOptionsCapacityRangeHasValueValidatorTests
    {
        private readonly CapacityTtlCacheServiceOptionsCapacityRangeHasValueValidator target;

        public CapacityTtlCacheServiceOptionsCapacityRangeHasValueValidatorTests()
        {
            target = new CapacityTtlCacheServiceOptionsCapacityRangeHasValueValidator();
        }

        [Fact]
        public void Validate_ShouldFail_WhenCapacityRangeIsNull()
        {
            var options = new CapacityTtlCacheServiceOptions
            {
                Capacity = 10
            };

            var result = target.Validate(null, options);

            Assert.False(result.Succeeded);
            Assert.Contains("CapacityRange", result.FailureMessage);
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

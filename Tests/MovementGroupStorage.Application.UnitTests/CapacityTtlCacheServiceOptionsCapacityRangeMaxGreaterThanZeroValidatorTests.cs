using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Infrastructure.Application.Services;

namespace MovementGroupStorage.Application.UnitTests
{
    public class CapacityTtlCacheServiceOptionsCapacityRangeMaxGreaterThanZeroValidatorTests
    {
        private readonly CapacityTtlCacheServiceOptionsCapacityRangeMaxGreaterThanZeroValidator target;

        public CapacityTtlCacheServiceOptionsCapacityRangeMaxGreaterThanZeroValidatorTests()
        {
            target = new CapacityTtlCacheServiceOptionsCapacityRangeMaxGreaterThanZeroValidator();
        }

        [Fact]
        public void Validate_ShouldFail_WhenMaxLessOrEqualZero()
        {
            var options = new CapacityTtlCacheServiceOptions
            {
                Capacity = 10,
                CapacityRange = new Range<int>
                {
                    Min = 5,
                    Max = 0
                }
            };

            var target = new CapacityTtlCacheServiceOptionsCapacityRangeMaxGreaterThanZeroValidator();

            var result = target.Validate(null, options);

            Assert.False(result.Succeeded);
            Assert.Contains("Max", result.FailureMessage);
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

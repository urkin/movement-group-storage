using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Infrastructure.Application.Services;

namespace MovementGroupStorage.Application.UnitTests
{
    public class CapacityTtlCacheServiceOptionsCapacityRangeMinGreaterThanZeroValidatorTests
    {
        private readonly CapacityTtlCacheServiceOptionsCapacityRangeMinGreaterThanZeroValidator target;

        public CapacityTtlCacheServiceOptionsCapacityRangeMinGreaterThanZeroValidatorTests()
        {
            target = new CapacityTtlCacheServiceOptionsCapacityRangeMinGreaterThanZeroValidator();
        }

        [Fact]
        public void Validate_ShouldFail_WhenMinLessOrEqualZero()
        {
            var options = new CapacityTtlCacheServiceOptions
            {
                Capacity = 10,
                CapacityRange = new Range<int>
                {
                    Min = 0,
                    Max = 20
                }
            };

            var target = new CapacityTtlCacheServiceOptionsCapacityRangeMinGreaterThanZeroValidator();

            var result = target.Validate(null, options);

            Assert.False(result.Succeeded);
            Assert.Contains("Min", result.FailureMessage);
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

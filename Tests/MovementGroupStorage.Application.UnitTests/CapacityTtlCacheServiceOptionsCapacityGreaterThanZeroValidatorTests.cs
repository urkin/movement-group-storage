using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Infrastructure.Application.Services;

namespace MovementGroupStorage.Application.UnitTests
{
    public class CapacityTtlCacheServiceOptionsCapacityGreaterThanZeroValidatorTests
    {
        private readonly CapacityTtlCacheServiceOptionsCapacityGreaterThanZeroValidator target;

        public CapacityTtlCacheServiceOptionsCapacityGreaterThanZeroValidatorTests()
        {
            this.target = new CapacityTtlCacheServiceOptionsCapacityGreaterThanZeroValidator();
        }

        [Fact]
        public void Validate_ShouldFail_WhenCapacityLessOrEqualZero()
        {
            var options = new CapacityTtlCacheServiceOptions
            {
                Capacity = 0,
                CapacityRange = new Range<int>
                {
                    Min = 5,
                    Max = 20
                }
            };

            var target = new CapacityTtlCacheServiceOptionsCapacityGreaterThanZeroValidator();

            var result = target.Validate(null, options);

            Assert.False(result.Succeeded);
            Assert.Contains("Capacity", result.FailureMessage);
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

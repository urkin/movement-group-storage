using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Infrastructure.Application.Services;

namespace MovementGroupStorage.Application.UnitTests
{
    public class CapacityTtlCacheServiceOptionsCapacityRangeCorrectValidatorTests
    {
        private readonly CapacityTtlCacheServiceOptionsCapacityRangeCorrectValidator target;

        public CapacityTtlCacheServiceOptionsCapacityRangeCorrectValidatorTests()
        {
            target = new CapacityTtlCacheServiceOptionsCapacityRangeCorrectValidator();
        }

        [Fact]
        public void Validate_ShouldFail_WhenMinGreaterThanMax()
        {
            var options = new CapacityTtlCacheServiceOptions
            {
                Capacity = 10,
                CapacityRange = new Range<int>
                {
                    Min = 30,
                    Max = 20
                }
            };
            options.CapacityRange.Min = 30;
            options.CapacityRange.Max = 20;

            var result = target.Validate(null, options);

            Assert.False(result.Succeeded);
            Assert.Contains("cannot be greater", result.FailureMessage);
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

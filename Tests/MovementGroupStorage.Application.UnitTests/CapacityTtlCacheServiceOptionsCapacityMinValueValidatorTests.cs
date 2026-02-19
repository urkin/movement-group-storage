using MovementGroupStorage.Application.Models;
using MovementGroupStorage.Infrastructure.Application.Services;

namespace MovementGroupStorage.Application.UnitTests
{
    public class CapacityTtlCacheServiceOptionsCapacityMinValueValidatorTests
    {
        private readonly CapacityTtlCacheServiceOptionsCapacityMinValueValidator target;

        public CapacityTtlCacheServiceOptionsCapacityMinValueValidatorTests()
        {
            target = new CapacityTtlCacheServiceOptionsCapacityMinValueValidator();
        }

        [Fact]
        public void Validate_ShouldFail_WhenCapacityLessThanMin()
        {
            var options = new CapacityTtlCacheServiceOptions
            {
                Capacity = 1,
                CapacityRange = new Range<int>
                {
                    Min = 5,
                    Max = 20
                }
            };

            var target = new CapacityTtlCacheServiceOptionsCapacityMinValueValidator();

            var result = target.Validate(null, options);

            Assert.False(result.Succeeded);
            Assert.Contains("greater than or equal", result.FailureMessage);
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

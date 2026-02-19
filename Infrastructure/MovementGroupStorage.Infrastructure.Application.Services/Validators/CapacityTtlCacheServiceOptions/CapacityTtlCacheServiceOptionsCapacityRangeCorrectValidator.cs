using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service used to validate <see cref="CapacityTtlCacheServiceOptions.CapacityRange.LowerBound"/> is lower than <see cref="CapacityTtlCacheServiceOptions.CapacityRange.UpperBound"/>.
    /// </summary>
    public class CapacityTtlCacheServiceOptionsCapacityRangeCorrectValidator : IValidateOptions<CapacityTtlCacheServiceOptions>
    {
        /// <summary>
        /// Validates whether <see cref="CapacityTtlCacheServiceOptions.CapacityRange.LowerBound"/> is lower than <see cref="CapacityTtlCacheServiceOptions.CapacityRange.UpperBound"/>.
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The <see cref="CapacityTtlCacheServiceOptions"/> instance.</param>
        /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
        public ValidateOptionsResult Validate(string? name, CapacityTtlCacheServiceOptions options)
        {
            if (options.CapacityRange.Min > options.CapacityRange.Max)
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CapacityRange.Min)} cannot be greater than {nameof(options.CapacityRange.Max)}");
            }

            return ValidateOptionsResult.Success;
        }
    }
}

using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service used to validate <see cref="CapacityTtlCacheServiceOptions.CapacityRange.Max"/> is greater than 0.
    /// </summary>
    public class CapacityTtlCacheServiceOptionsCapacityRangeMaxGreaterThanZeroValidator : IValidateOptions<CapacityTtlCacheServiceOptions>
    {
        /// <summary>
        /// Validates whether <see cref="CapacityTtlCacheServiceOptions.CapacityRange.Max"/> is greater than 0.
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The <see cref="CapacityTtlCacheServiceOptions"/> instance.</param>
        /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
        public ValidateOptionsResult Validate(string? name, CapacityTtlCacheServiceOptions options)
        {
            if (options.CapacityRange.Max <= 0)
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CapacityRange.Max)} must be greater than 0");
            }

            return ValidateOptionsResult.Success;
        }
    }
}

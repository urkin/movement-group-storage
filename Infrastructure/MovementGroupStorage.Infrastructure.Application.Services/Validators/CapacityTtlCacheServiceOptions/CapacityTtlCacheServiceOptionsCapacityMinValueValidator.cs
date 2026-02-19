using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service used to validate <see cref="CapacityTtlCacheServiceOptions.Capacity"/> is lower than <see cref="CapacityTtlCacheServiceOptions.CapacityRange.LowerBound"/>>.
    /// </summary>
    public class CapacityTtlCacheServiceOptionsCapacityMinValueValidator : IValidateOptions<CapacityTtlCacheServiceOptions>
    {
        /// <summary>
        /// Validates whether <see cref="CapacityTtlCacheServiceOptions.Capacity"/> is lower than <see cref="CapacityTtlCacheServiceOptions.CapacityRange.LowerBound"/>.
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The <see cref="CapacityTtlCacheServiceOptions"/> instance.</param>
        /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
        public ValidateOptionsResult Validate(string? name, CapacityTtlCacheServiceOptions options)
        {
            if (options.Capacity < options.CapacityRange.Min)
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Capacity)} must be greater than or equal {options.CapacityRange.Min}");
            }

            return ValidateOptionsResult.Success;
        }
    }
}

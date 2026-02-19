using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service used to validate <see cref="CapacityTtlCacheServiceOptions.Capacity"/> is greater
    /// than <see cref="CapacityTtlCacheServiceOptions.CapacityRange.UpperBound"/>.
    /// </summary>
    public class CapacityTtlCacheServiceOptionsCapacityMaxValueValidator : IValidateOptions<CapacityTtlCacheServiceOptions>
    {
        /// <summary>
        /// Validates whether <see cref="CapacityTtlCacheServiceOptions.Capacity"/> is greater than <see cref="CapacityTtlCacheServiceOptions.CapacityRange.UpperBound"/>.
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The <see cref="CapacityTtlCacheServiceOptions"/> instance.</param>
        /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
        public ValidateOptionsResult Validate(string? name, CapacityTtlCacheServiceOptions options)
        {
            if (options.Capacity > options.CapacityRange.Max)
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Capacity)} must be less than or equal {options.CapacityRange.Max}");
            }

            return ValidateOptionsResult.Success;
        }
    }
}

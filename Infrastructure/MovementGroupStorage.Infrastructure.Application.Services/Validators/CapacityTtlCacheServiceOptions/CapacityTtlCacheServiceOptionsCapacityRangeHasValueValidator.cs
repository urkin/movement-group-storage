using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service used to validate <see cref="CapacityTtlCacheServiceOptions.CapacityRange"/> is not null.
    /// </summary>
    public class CapacityTtlCacheServiceOptionsCapacityRangeHasValueValidator : IValidateOptions<CapacityTtlCacheServiceOptions>
    {
        /// <summary>
        /// Validates whether <see cref="CapacityTtlCacheServiceOptions.CapacityRange"/> is not null.
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The <see cref="CapacityTtlCacheServiceOptions"/> instance.</param>
        /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
        public ValidateOptionsResult Validate(string? name, CapacityTtlCacheServiceOptions options)
        {
            if (options.CapacityRange == null)
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CapacityRange)} must be provided");
            }

            return ValidateOptionsResult.Success;
        }
    }
}

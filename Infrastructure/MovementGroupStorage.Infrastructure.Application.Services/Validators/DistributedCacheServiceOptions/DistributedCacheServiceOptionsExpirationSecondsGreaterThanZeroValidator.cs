using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service used to validate <see cref="DistributedCacheServiceOptions.ExpirationSeconds"/> is greater than 0.
    /// </summary>
    public class DistributedCacheServiceOptionsExpirationSecondsGreaterThanZeroValidator : IValidateOptions<DistributedCacheServiceOptions>
    {
        /// <summary>
        /// Validates whether <see cref="DistributedCacheServiceOptions.ExpirationSeconds"/> is greater than 0.
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The <see cref="DistributedCacheServiceOptions"/> instance.</param>
        /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
        public ValidateOptionsResult Validate(string? name, DistributedCacheServiceOptions options)
        {
            if (options.ExpirationSeconds <= 0)
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ExpirationSeconds)} must be greater than 0");
            }

            return ValidateOptionsResult.Success;
        }
    }
}

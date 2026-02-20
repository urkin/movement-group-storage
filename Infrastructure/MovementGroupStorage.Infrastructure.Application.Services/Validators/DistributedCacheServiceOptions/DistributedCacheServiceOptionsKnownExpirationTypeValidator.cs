using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service used to validate <see cref="DistributedCacheServiceOptions.ExpirationType"/> is known.
    /// </summary>
    public class DistributedCacheServiceOptionsKnownExpirationTypeValidator : IValidateOptions<DistributedCacheServiceOptions>
    {
        /// <summary>
        /// Validates whether <see cref="DistributedCacheServiceOptions.ExpirationType"/> is known.
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The <see cref="DistributedCacheServiceOptions"/> instance.</param>
        /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
        public ValidateOptionsResult Validate(string? name, DistributedCacheServiceOptions options)
        {
            if (options.ExpirationType == ExpirationType.Unknown)
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ExpirationType)} can not be {nameof(ExpirationType.Unknown)}");
            }

            return ValidateOptionsResult.Success;
        }
    }
}

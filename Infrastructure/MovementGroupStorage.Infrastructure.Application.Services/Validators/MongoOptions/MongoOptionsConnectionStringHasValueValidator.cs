using Microsoft.Extensions.Options;
using MovementGroupStorage.Application.Models;

namespace MovementGroupStorage.Infrastructure.Application.Services
{
    /// <summary>
    /// Service used to validate <see cref="MongoOptions.ConnectionString"/> is not null or white space.
    /// </summary>
    public class MongoOptionsConnectionStringHasValueValidator : IValidateOptions<MongoOptions>
    {
        /// <summary>
        /// Validates whether <see cref="MongoOptions.ConnectionString"/> is not null.
        /// </summary>
        /// <param name="name">The name of the options instance being validated.</param>
        /// <param name="options">The <see cref="MongoOptions"/> instance.</param>
        /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
        public ValidateOptionsResult Validate(string? name, MongoOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ConnectionString)} must be provided");
            }

            return ValidateOptionsResult.Success;
        }
    }
}

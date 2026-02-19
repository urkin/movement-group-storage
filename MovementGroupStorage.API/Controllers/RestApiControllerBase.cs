using Microsoft.AspNetCore.Mvc;
using MovementGroupStorage.Application.Models;
using System.Net;

namespace MovementGroupStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class RestApiControllerBase : ControllerBase
    {
        protected readonly IServiceProvider _serviceProvider;

        protected RestApiControllerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        protected IActionResult ResolveResponse(ApplicationServiceResult result)
        {
            var resolveStatus = _serviceProvider
                .GetRequiredKeyedService<Func<HttpStatusCode>>(result.Status);
            var statusCode = (int)resolveStatus();

            return StatusCode(statusCode, result.Data);
        }
    }
}

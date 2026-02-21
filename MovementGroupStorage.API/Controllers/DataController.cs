using Microsoft.AspNetCore.Mvc;
using MovementGroupStorage.Application.Managers;

namespace MovementGroupStorage.API.Controllers
{
    public class DataController : RestApiControllerBase
    {
        private readonly IDataManager _dataManager;

        public DataController(IDataManager dataManager, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _dataManager.FetchAsync(id);
            var response = ResolveResponse(result);
            return response;
        }

        [HttpPost]
        public async Task<IActionResult> Post(dynamic data)
        {
            var result = await _dataManager.CreateAsync(data);
            var response = ResolveResponse(result);
            return response;
        }
    }
}

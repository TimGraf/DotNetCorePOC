using DotNetCouchbasePOC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Swashbuckle.SwaggerGen.Annotations;

namespace DotNetCouchbasePOC.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BeersController : Controller
    {
        private readonly ILogger<BeersController> _logger;
        private IBeerService _beerService;

        public BeersController(ILogger<BeersController> logger, IBeerService beerService)
        {
            _logger      = logger;
            _beerService = beerService;
        }

        // GET api/beers?query=blueberry
        [HttpGet]
        [SwaggerOperation("SearchBeers")]
        [SwaggerResponse(200)]
        public async Task<IActionResult> SearchBeers(string queryString, int pageSize = 10, int offset = 0)
        {
            _logger.LogInformation("Query string: " + queryString);

            string result = await _beerService.searchBeers(queryString, pageSize, offset);

            return Ok(result);
        }
    }
}
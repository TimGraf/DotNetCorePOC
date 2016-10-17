using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using DotNetCouchbasePOC.Models;

namespace DotNetCouchbasePOC.Services
{
    public interface IBeerService
    {
        Task<string> searchBeers(string queryString, int pageSize, int offset);
    }

    public class BeerService: IBeerService
    {
        private readonly ILogger<BeerService> _logger;
        private ISearchService _serchService;
        private IDocumentService _documentService;

        public BeerService(ILogger<BeerService> logger, ISearchService serchService, IDocumentService documentService)
        {
            _logger          = logger;
            _serchService    = serchService;
            _documentService = documentService;

            _logger.LogInformation("Initialized Beer Service");
        }

        public async Task<string> searchBeers(string queryString, int pageSize, int offset)
        {
            IEnumerable<string> ids                 = _serchService.search(queryString, pageSize, offset);
            IEnumerable<CBResult<Beer>> futureBeers = await _documentService.get<CBResult<Beer>>(ids);
            
            return JsonConvert.SerializeObject(futureBeers);
        }
    }
}
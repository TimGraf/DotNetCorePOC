using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Nest;
using DotNetCouchbasePOC.Config;
using DotNetCouchbasePOC.Models;

namespace DotNetCouchbasePOC.Services
{
    public interface ISearchService
    {
	    IEnumerable<String> search(string queryString, int pageSize, int offSet);
    }

    public class SearchService : ISearchService
    {
        private readonly ILogger<SearchService> _logger;
        private string serviceUrl;
        private string serahcIndex;
        private Uri node;
        private ConnectionSettings esConfig;
        private ElasticClient esClient;

        public SearchService(IOptions<SearchServiceConfig> searchServiceConfig, ILogger<SearchService> logger)
        {
            _logger     = logger;
            serviceUrl  = searchServiceConfig.Value.serviceUrl;
            serahcIndex = searchServiceConfig.Value.searchIndex;

            _logger.LogInformation("Search service URL: " + serviceUrl);
            _logger.LogInformation("Search service index: " + serahcIndex);

            node        = new Uri(serviceUrl + serahcIndex);
            esConfig    = new ConnectionSettings(node);
            esClient    = new ElasticClient(esConfig);
        }

        /*
         * Search Elasticsearch using a query string and return the CouchBase document IDs
         * that match the query. optional parameters for page size and offset with default
         * values of 10 and 0 respectively. 
         */
        public IEnumerable<String> search(string queryString, int pageSize, int offset)
        {
            _logger.LogDebug("Search service query string: " + queryString);
            _logger.LogDebug("Search service page size: " + pageSize);
            _logger.LogDebug("Search service offset size: " + offset);

            var response = esClient.Search<ESResult>(s => s
                .From(offset)
                .Size(pageSize)
                .Query(q => q
                    .QueryString(qs => qs
                        .Query(queryString + "*")
                    )
                )
            );

            _logger.LogDebug("search result hits: " + response.Hits.Count());

            return response.Documents.ToList().Select(x => x.meta.id);
        }
    }
}
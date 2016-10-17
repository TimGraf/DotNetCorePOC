using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using DotNetCouchbasePOC.Config;

namespace DotNetCouchbasePOC.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<T>> get<T>(IEnumerable<string> ids);
    }

    public class DocumentService : IDocumentService
    {
        private readonly ILogger<DocumentService> _logger;
        private string serviceUrl;
        private string apiPrefix = "pools/default/buckets/";
        private string bucket = "beer-sample";
        HttpClient client;

        public DocumentService(IOptions<DocumentServiceConfig> documentServiceConfig, ILogger<DocumentService> logger)
        {
            _logger    = logger;
            serviceUrl = documentServiceConfig.Value.documentServiceUrl;

            _logger.LogInformation("Beer service URL: " + serviceUrl);

            client     = new HttpClient();
        }

        public async Task<IEnumerable<T>> get<T>(IEnumerable<string> ids) {
            List<T> items = new List<T>();

            foreach (var id in ids)
            {
                string itemJson = await client.GetStringAsync(serviceUrl + apiPrefix + bucket + "/docs/" + id);

                _logger.LogDebug(itemJson);

                T item = JsonConvert.DeserializeObject<T>(itemJson);

                items.Add(item);
            }

            return items;
        }
    }
}
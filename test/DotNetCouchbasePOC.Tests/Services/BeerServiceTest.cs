using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DotNetCouchbasePOC.Models;

namespace DotNetCouchbasePOC.Services.UnitTests
{
    public class BeearServiceTest
    {
        private readonly BeerService _beerService;
         public BeearServiceTest()
         { 
             // Mock services
             var mockLogger          = new Mock<ILogger<BeerService>>();
             var mockSearchService   = new Mock<ISearchService>();
             var mockDocumentService = new Mock<IDocumentService>();

             // Stub calls
             mockSearchService.Setup(search => search.search("some query string", 10, 0)).Returns(Enumerable.Empty<string>());
             mockDocumentService.Setup(doc => doc.get<CBResult<Beer>>(Enumerable.Empty<string>())).Returns(Task.FromResult(Enumerable.Empty<CBResult<Beer>>()));

             // Create beer service
             _beerService = new BeerService(mockLogger.Object, mockSearchService.Object, mockDocumentService.Object); 
         } 
        
        [Fact]
        public async void someBeerServiceTest() {
            string result = await _beerService.searchBeers("some query string", 10, 0);

            Assert.Equal("[]", result);
        }
    }
}
using HMS.Core.Services.Catalog;
using System.Threading.Tasks;
using Xunit;

namespace HMS.UnitTests
{
    public class BasketServiceTests
    {
        [Fact]
        public async Task GetFakeBasketTest()
        {
			CatalogMockService catalogMockService = new CatalogMockService();
			System.Collections.ObjectModel.ObservableCollection<Core.Models.Catalog.CatalogItem> result  = await catalogMockService.GetCatalogAsync();
            Assert.NotEmpty(result);
        }
    }
}
using HMS.Core.Services.Catalog;
using System.Threading.Tasks;
using Xunit;

namespace HMS.UnitTests
{
    public class CatalogServiceTests
    {
        [Fact]
        public async Task GetFakeCatalogTest()
        {
			CatalogMockService catalogMockService = new CatalogMockService();
			System.Collections.ObjectModel.ObservableCollection<Core.Models.Catalog.CatalogItem> catalog = await catalogMockService.GetCatalogAsync();

            Assert.NotEmpty(catalog);
        }

        [Fact]
        public async Task GetFakeCatalogBrandTest()
        {
			CatalogMockService catalogMockService = new CatalogMockService();
			System.Collections.ObjectModel.ObservableCollection<Core.Models.Catalog.CatalogBrand> catalogBrand = await catalogMockService.GetCatalogBrandAsync();

            Assert.NotEmpty(catalogBrand);
        }

        [Fact]
        public async Task GetFakeCatalogTypeTest()
        {
			CatalogMockService catalogMockService = new CatalogMockService();
			System.Collections.ObjectModel.ObservableCollection<Core.Models.Catalog.CatalogType> catalogType = await catalogMockService.GetCatalogTypeAsync();

            Assert.NotEmpty(catalogType);
        }
    }
}
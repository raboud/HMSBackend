using HMS.Core;
using HMS.Core.Services.Order;
using System.Threading.Tasks;
using Xunit;

namespace HMS.UnitTests
{
    public class OrdersServiceTests
    {
		[Fact]
		public async Task GetFakeOrderTest()
		{
			OrderMockService ordersMockService = new OrderMockService();
			Core.Models.Orders.Order order = await ordersMockService.GetOrderAsync(1, GlobalSetting.Instance.AuthToken);

			Assert.NotNull(order);
		}
		
        [Fact]
        public async Task GetFakeOrdersTest()
        {
			OrderMockService ordersMockService = new OrderMockService();
			System.Collections.ObjectModel.ObservableCollection<Core.Models.Orders.Order> result = await ordersMockService.GetOrdersAsync(GlobalSetting.Instance.AuthToken);

            Assert.NotEmpty(result);
        }
    }
}
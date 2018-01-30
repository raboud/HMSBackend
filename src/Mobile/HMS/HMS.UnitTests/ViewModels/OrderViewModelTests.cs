using Xunit;
using HMS.Core;
using HMS.Core.ViewModels;
using HMS.Core.ViewModels.Base;
using HMS.Core.Services.Order;
using System.Threading.Tasks;

namespace HMS.UnitTests
{
	public class OrderViewModelTests
	{
		public OrderViewModelTests()
		{
			ViewModelLocator.RegisterDependencies(true);
		}

		[Fact]
		public void OrderPropertyIsNullWhenViewModelInstantiatedTest()
		{
			var orderService = new OrderMockService();
			var orderViewModel = new OrderDetailViewModel(orderService);
			Assert.Null(orderViewModel.Order);
		}

		[Fact]
		public async Task OrderPropertyIsNotNullAfterViewModelInitializationTest()
		{
			var orderService = new OrderMockService();
			var orderViewModel = new OrderDetailViewModel(orderService);

			var order = await orderService.GetOrderAsync(1, GlobalSetting.Instance.AuthToken);
			await orderViewModel.InitializeAsync(order);

			Assert.NotNull(orderViewModel.Order);
		}

		[Fact]
		public async Task SettingOrderPropertyShouldRaisePropertyChanged()
		{
			bool invoked = false;
			var orderService = new OrderMockService();
			var orderViewModel = new OrderDetailViewModel(orderService);

			orderViewModel.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName.Equals("Order"))
					invoked = true;
			};
			var order = await orderService.GetOrderAsync(1, GlobalSetting.Instance.AuthToken);
			await orderViewModel.InitializeAsync(order);

			Assert.True(invoked);
		}
	}
}
using System;
using System.Threading.Tasks;
using HMS.Core.Services.RequestProvider;
using HMS.Core.Models.Basket;
using HMS.Core.Helpers;

namespace HMS.Core.Services.Basket
{
    public class BasketService : IBasketService
    {
        private readonly IRequestProvider _requestProvider;
        private const string ApiUrlBase = "api/v1/basket";

        public BasketService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<CustomerBasket> GetBasketAsync(string guidUser, string token)
        {
            var builder = new UriBuilder(GlobalSetting.Instance.BasketEndpoint)
            {
                Path = $"{ApiUrlBase}/{guidUser}"
            };

            var uri = builder.ToString();

            CustomerBasket basket =
                    await _requestProvider.GetAsync<CustomerBasket>(uri, token);

            ServicesHelper.FixBasketItemPictureUri(basket?.Items);

            return basket;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket, string token)
        {
            var builder = new UriBuilder(GlobalSetting.Instance.BasketEndpoint)
            {
                Path = ApiUrlBase
            };

            var uri = builder.ToString();

            var result = await _requestProvider.PostAsync(uri, customerBasket, token);

            return result;
        }

        public async Task CheckoutAsync(BasketCheckout basketCheckout, string token)
        {
            var builder = new UriBuilder(GlobalSetting.Instance.BasketEndpoint)
            {
                Path = $"{ApiUrlBase}/checkout"
            };

            var uri = builder.ToString();

            await _requestProvider.PostAsync(uri, basketCheckout, token);
        }

        public async Task ClearBasketAsync(string guidUser, string token)
        {
            var builder = new UriBuilder(GlobalSetting.Instance.BasketEndpoint)
            {
                Path = $"{ApiUrlBase}/{guidUser}"
            };

            var uri = builder.ToString();

            await _requestProvider.DeleteAsync(uri, token);
        }
    }
}
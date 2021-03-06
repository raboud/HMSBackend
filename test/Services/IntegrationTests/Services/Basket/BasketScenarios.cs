﻿using BasketAPI.Model;
using IntegrationTests.Services.Extensions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebMVC.Models;
using Xunit;

namespace IntegrationTests.Services.Basket
{
    public class BasketScenarios
        : BasketScenarioBase
    {
        [Fact]
        public async Task Post_basket_and_response_ok_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				StringContent content = new StringContent(BuildBasket(), UTF8Encoding.UTF8, "application/json");
				HttpResponseMessage response = await server.CreateClient()
                   .PostAsync(Post.Basket, content);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_basket_and_response_ok_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				HttpResponseMessage response = await server.CreateClient()
                   .GetAsync(Get.GetBasket(1));

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Send_Checkout_basket_and_response_ok_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				StringContent contentBasket = new StringContent(BuildBasket(), UTF8Encoding.UTF8, "application/json");
                await server.CreateClient()
                   .PostAsync(Post.Basket, contentBasket);

				StringContent contentCheckout = new StringContent(BuildCheckout(), UTF8Encoding.UTF8, "application/json");
				HttpResponseMessage response = await server.CreateIdempotentClient()
                   .PostAsync(Post.CheckoutOrder, contentCheckout);

                response.EnsureSuccessStatusCode();
            }
        }

        string BuildBasket()
        {
			CustomerBasket order = new CustomerBasket("1234");            
            return JsonConvert.SerializeObject(order);
        }

        string BuildCheckout()
        {
			BasketDTO checkoutBasket = new BasketDTO()
            {
                City = "city",
                Street = "street",
                State = "state",
                Country = "coutry",
                ZipCode = "zipcode",
                CardNumber = "CardNumber",
                CardHolderName = "CardHolderName",
                CardExpiration = DateTime.UtcNow,
                CardSecurityNumber = "1234",
                CardTypeId = 1,
                Buyer = "Buyer",
                RequestId = Guid.NewGuid()
            };

            return JsonConvert.SerializeObject(checkoutBasket);
        }
    }
}

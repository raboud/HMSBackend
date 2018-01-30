﻿using FunctionalTests.Extensions;
using FunctionalTests.Services.Basket;
using Basket.API.Model;
using Microsoft.WebMVC.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebMVC.Models;
using Xunit;
using BasketAPI.Model;

namespace FunctionalTests.Services.Ordering
{
    public class OrderingScenarios : OrderingScenariosBase
    {        
        [Fact]
        public async Task Cancel_basket_and_check_order_status_cancelled()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer orderServer = new OrderingScenariosBase().CreateServer())
            using (Microsoft.AspNetCore.TestHost.TestServer basketServer = new BasketScenariosBase().CreateServer())
            {
				// Expected data
				string cityExpected = $"city-{Guid.NewGuid()}";
				string orderStatusExpected = "cancelled";

				HttpClient basketClient = basketServer.CreateIdempotentClient();
				HttpClient orderClient = orderServer.CreateIdempotentClient();

				// GIVEN a basket is created 
				StringContent contentBasket = new StringContent(BuildBasket(), UTF8Encoding.UTF8, "application/json");
                await basketClient.PostAsync(BasketScenariosBase.Post.CreateBasket, contentBasket);

                // AND basket checkout is sent
                await basketClient.PostAsync(BasketScenariosBase.Post.Checkout, new StringContent(BuildCheckout(cityExpected), UTF8Encoding.UTF8, "application/json"));

				// WHEN Order is created in Ordering.api
				Order newOrder = await TryGetNewOrderCreated(cityExpected, orderClient);

                // AND Order is cancelled in Ordering.api
                await orderClient.PutAsync(OrderingScenariosBase.Put.CancelOrder, new StringContent(BuildCancelOrder(newOrder.OrderNumber), UTF8Encoding.UTF8, "application/json"));

				// AND the requested order is retrieved
				Order order = await TryGetNewOrderCreated(cityExpected, orderClient);

                // THEN check status
                Assert.Equal(orderStatusExpected, order.Status);
            }
        }

        private async Task<Order> TryGetNewOrderCreated(string city, HttpClient orderClient)
        {
			int counter = 0;
            Order order = null;

            while (counter < 20)
            {
				//get the orders and verify that the new order has been created
				string ordersGetResponse = await orderClient.GetStringAsync(OrderingScenariosBase.Get.Orders);
				List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(ordersGetResponse);

                if (orders == null || orders.Count == 0) {
                    counter++;
                    await Task.Delay(100);
                    continue;
                }

				Order lastOrder = orders.OrderByDescending(o => o.Date).First();
                int.TryParse(lastOrder.OrderNumber, out int id);
				string orderDetails = await orderClient.GetStringAsync(OrderingScenariosBase.Get.OrderBy(id));
                order = JsonConvert.DeserializeObject<Order>(orderDetails);

                if (IsOrderCreated(order, city))
                {
                    break;
                }                
            }                
            
            return order;
        }

        private bool IsOrderCreated(Order order, string city)
        {
            return order.City == city;
        }

        string BuildBasket()
        {
			CustomerBasket order = new CustomerBasket("9e3163b9-1ae6-4652-9dc6-7898ab7b7a00");
            order.Items = new List<BasketAPI.Model.BasketItem>()
            {
                new BasketAPI.Model.BasketItem()
                {
                    Id = "1",
                    ProductName = "ProductName",
                    ProductId = "1",
                    UnitPrice = 10,
                    Quantity = 1
                }
            };
            return JsonConvert.SerializeObject(order);
        }

        string BuildCancelOrder(string orderId)
        {
			OrderDTO order = new OrderDTO()
            {
                OrderNumber = orderId
            };           
            return JsonConvert.SerializeObject(order);
        }

        string BuildCheckout(string cityExpected)
        {
			BasketDTO checkoutBasket = new BasketDTO()
            {
                City = cityExpected,
                Street = "street",
                State = "state",
                Country = "coutry",
                ZipCode = "zipcode",
                CardNumber = "1111111111111",
                CardHolderName = "CardHolderName",
                CardExpiration = DateTime.Now.AddYears(1),
                CardSecurityNumber = "123",
                CardTypeId = 1,
                Buyer = "Buyer",                
                RequestId = Guid.NewGuid()
            };

            return JsonConvert.SerializeObject(checkoutBasket);
        }
    }
}
 
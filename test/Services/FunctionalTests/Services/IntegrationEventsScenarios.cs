﻿using FunctionalTests.Services.Basket;
using FunctionalTests.Services.Catalog;
using HMS.Catalog.API.Model;
using HMS.Catalog.API.ViewModel;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using System.Threading;
using BasketAPI.Model;

namespace FunctionalTests.Services
{
    public class IntegrationEventsScenarios
    {
        [Fact]
        public async Task Post_update_product_price_and_catalog_and_basket_list_modified()
        {
            decimal priceModification = 0.15M;
            string userId = "JohnId";

            using (Microsoft.AspNetCore.TestHost.TestServer catalogServer = new CatalogScenariosBase().CreateServer())
            using (Microsoft.AspNetCore.TestHost.TestServer basketServer = new BasketScenariosBase().CreateServer())
            {
				HttpClient catalogClient = catalogServer.CreateClient();
				HttpClient basketClient = basketServer.CreateClient();

				// GIVEN a product catalog list                           
				PaginatedItemsViewModel<Product> originalCatalogProducts = await GetCatalogAsync(catalogClient);

				// AND a user basket filled with products   
				CustomerBasket basket = ComposeBasket(userId, originalCatalogProducts.Data.Take(3));
				HttpResponseMessage res = await basketClient.PostAsync(
                    BasketScenariosBase.Post.CreateBasket,
                    new StringContent(JsonConvert.SerializeObject(basket), UTF8Encoding.UTF8, "application/json")
                    );

				// WHEN the price of one product is modified in the catalog
				BasketItem itemToModify = basket.Items[2];
				decimal oldPrice = itemToModify.UnitPrice;
				decimal newPrice = oldPrice + priceModification;
				HttpResponseMessage pRes = await catalogClient.PutAsync(CatalogScenariosBase.Put.UpdateCatalogProduct, new StringContent(ChangePrice(itemToModify, newPrice, originalCatalogProducts), UTF8Encoding.UTF8, "application/json"));

				PaginatedItemsViewModel<Product> modifiedCatalogProducts = await GetCatalogAsync(catalogClient);

				BasketItem itemUpdated = await GetUpdatedBasketItem(newPrice, itemToModify.ProductId, userId, basketClient);

                if (itemUpdated == null)
                {
                    Assert.False(true, $"The basket service has not been updated.");
                }
                else
                {
                    //THEN the product price changes in the catalog 
                    Assert.Equal(newPrice, modifiedCatalogProducts.Data.Single(it => it.Id == int.Parse(itemToModify.ProductId)).Price);

                    // AND the products in the basket reflects the changed priced and the original price
                    Assert.Equal(newPrice, itemUpdated.UnitPrice);
                    Assert.Equal(oldPrice, itemUpdated.OldUnitPrice);
                }
            }
        }

        private async Task<BasketItem> GetUpdatedBasketItem(decimal newPrice, string productId, string userId, HttpClient basketClient)
        {
            bool continueLoop = true;
			int counter = 0;
            BasketItem itemUpdated = null;

            while (continueLoop && counter < 20)
            {
				//get the basket and verify that the price of the modified product is updated
				HttpResponseMessage basketGetResponse = await basketClient.GetAsync(BasketScenariosBase.Get.GetBasketByCustomer(userId));
				CustomerBasket basketUpdated = JsonConvert.DeserializeObject<CustomerBasket>(await basketGetResponse.Content.ReadAsStringAsync());

                itemUpdated = basketUpdated.Items.Single(pr => pr.ProductId == productId);

                if (itemUpdated.UnitPrice == newPrice)
                {
                    continueLoop = false;
                }
                else
                {
                    counter++;
                    await Task.Delay(100);
                }
            }

            return itemUpdated;
        }

        private async  Task<PaginatedItemsViewModel<Product>> GetCatalogAsync(HttpClient catalogClient)
        {
			HttpResponseMessage response = await catalogClient.GetAsync(CatalogScenariosBase.Get.Items);
			string items = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PaginatedItemsViewModel<Product>>(items);
        }

        private string ChangePrice(BasketItem itemToModify, decimal newPrice, PaginatedItemsViewModel<Product> catalogProducts)
        {
			Product catalogProduct = catalogProducts.Data.Single(pr => pr.Id == int.Parse(itemToModify.ProductId));
            catalogProduct.Price = newPrice;
            return JsonConvert.SerializeObject(catalogProduct);
        }

        private CustomerBasket ComposeBasket(string customerId, IEnumerable<Product> items)
        {
			CustomerBasket basket = new CustomerBasket(customerId);
            foreach (Product item in items)
            {
                basket.Items.Add(new BasketItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    UnitPrice = item.Price,
                    PictureUrl = item.PictureUri,
                    ProductId = item.Id.ToString(),
                    OldUnitPrice = 0,
                    ProductName = item.Name,
                    Quantity = 1
                });
            }
            return basket;
        }
    }
}
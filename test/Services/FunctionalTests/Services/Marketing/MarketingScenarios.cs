using UserLocation = Locations.API.Model.UserLocation;
using LocationRequest = Locations.API.ViewModel.LocationRequest;
using FunctionalTests.Services.Locations;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using Marketing.API.Dto;
using HMS.Catalog.API.ViewModel;

namespace FunctionalTests.Services.Marketing
{
    public class MarketingScenarios : MarketingScenariosBase
    {
        [Fact]
        public async Task Set_new_user_location_and_get_location_campaign_by_user_id()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer locationsServer = new LocationsScenariosBase().CreateServer())
            using (Microsoft.AspNetCore.TestHost.TestServer marketingServer = new MarketingScenariosBase().CreateServer())
            {
				LocationRequest location = new LocationRequest
                {
                    Longitude = -122.315752,
                    Latitude = 47.60461
                };
				StringContent content = new StringContent(JsonConvert.SerializeObject(location),
                    Encoding.UTF8, "application/json");

                // GIVEN a new location of user is created 
                await locationsServer.CreateClient()
                    .PostAsync(LocationsScenariosBase.Post.AddNewLocation, content);

                await Task.Delay(300);

				//Get campaing from Marketing.API
				HttpResponseMessage campaignsResponse = await marketingServer.CreateClient()
                    .GetAsync(CampaignScenariosBase.Get.Campaigns);

				string responseBody = await campaignsResponse.Content.ReadAsStringAsync();
				List<CampaignDTO> campaigns = JsonConvert.DeserializeObject<List<CampaignDTO>>(responseBody);

                Assert.True(campaigns.Count > 0);
            }
        }
    }
}

using Locations.API.Model;
using Locations.API.ViewModel;
using Location = Locations.API.Model.Locations;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System;

namespace IntegrationTests.Services.Locations
{
    public class LocationsScenarios
        : LocationsScenarioBase
    {
        [Fact]
        public async Task Set_new_user_seattle_location_response_ok_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				string userId = "4611ce3f-380d-4db5-8d76-87a8689058ed";
				StringContent content = new StringContent(BuildLocationsRequest(-122.315752, 47.604610), UTF8Encoding.UTF8, "application/json");

				// Expected result
				string expectedLocation = "SEAT";

				// Act
				HttpResponseMessage response = await server.CreateClient()
                    .PostAsync(Post.AddNewLocation, content);

				HttpResponseMessage userLocationResponse = await server.CreateClient()
                    .GetAsync(Get.UserLocationBy(userId));

				string responseBody = await userLocationResponse.Content.ReadAsStringAsync();
				UserLocation userLocation = JsonConvert.DeserializeObject<UserLocation>(responseBody);

				HttpResponseMessage locationResponse = await server.CreateClient()
                    .GetAsync(Get.LocationBy(userLocation.LocationId));

                responseBody = await locationResponse.Content.ReadAsStringAsync();
				Location location = JsonConvert.DeserializeObject<Location>(responseBody);

                // Assert
                Assert.Equal(expectedLocation, location.Code);
            }
        }

        [Fact]
        public async Task Set_new_user_readmond_location_response_ok_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				string userId = "4611ce3f-380d-4db5-8d76-87a8689058ed";
				StringContent content = new StringContent(BuildLocationsRequest(-122.119998, 47.690876), UTF8Encoding.UTF8, "application/json");

				// Expected result
				string expectedLocation = "REDM";

				// Act
				HttpResponseMessage response = await server.CreateClient()
                    .PostAsync(Post.AddNewLocation, content);

				HttpResponseMessage userLocationResponse = await server.CreateClient()
                    .GetAsync(Get.UserLocationBy(userId));

				string responseBody = await userLocationResponse.Content.ReadAsStringAsync();
				UserLocation userLocation = JsonConvert.DeserializeObject<UserLocation>(responseBody);

				HttpResponseMessage locationResponse = await server.CreateClient()
                    .GetAsync(Get.LocationBy(userLocation.LocationId));

                responseBody = await locationResponse.Content.ReadAsStringAsync();
				Location location = JsonConvert.DeserializeObject<Location>(responseBody);

                // Assert
                Assert.Equal(expectedLocation, location.Code);
            }
        }

        [Fact]
        public async Task Set_new_user_Washington_location_response_ok_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				string userId = "4611ce3f-380d-4db5-8d76-87a8689058ed";
				StringContent content = new StringContent(BuildLocationsRequest(-121.040360, 48.091631), UTF8Encoding.UTF8, "application/json");

				// Expected result
				string expectedLocation = "WHT";

				// Act
				HttpResponseMessage response = await server.CreateClient()
                    .PostAsync(Post.AddNewLocation, content);

				HttpResponseMessage userLocationResponse = await server.CreateClient()
                    .GetAsync(Get.UserLocationBy(userId));

				string responseBody = await userLocationResponse.Content.ReadAsStringAsync();
				UserLocation userLocation = JsonConvert.DeserializeObject<UserLocation>(responseBody);

				HttpResponseMessage locationResponse = await server.CreateClient()
                    .GetAsync(Get.LocationBy(userLocation.LocationId));

                responseBody = await locationResponse.Content.ReadAsStringAsync();
				Location location = JsonConvert.DeserializeObject<Location>(responseBody);

                // Assert
                Assert.Equal(expectedLocation, location.Code);
            }
        }

        [Fact]
        public async Task Get_all_locations_response_ok_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				HttpResponseMessage response = await server.CreateClient()
                    .GetAsync(Get.Locations);

				string responseBody = await response.Content.ReadAsStringAsync();
				List<Location> locations = JsonConvert.DeserializeObject<List<Location>>(responseBody);

                // Assert
                Assert.NotEmpty(locations);
            }
        }

        string BuildLocationsRequest(double lon, double lat)
        {
			LocationRequest location = new LocationRequest()
            {
                Longitude = lon,
                Latitude = lat
            }; 
            return JsonConvert.SerializeObject(location);
        }
    }
}

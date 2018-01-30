using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System;
using Newtonsoft.Json;
using System.Net;
using Marketing.API.Dto;

namespace IntegrationTests.Services.Marketing
{

    public class CampaignScenarios
        : CampaignScenarioBase
    {
        [Fact]
        public async Task Get_get_all_campaigns_and_response_ok_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				HttpResponseMessage response = await server.CreateClient()
                    .GetAsync(Get.Campaigns);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_get_campaign_by_id_and_response_ok_status_code()
        {
			int campaignId = 1;
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				HttpResponseMessage response = await server.CreateClient()
                    .GetAsync(Get.CampaignBy(campaignId));

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_get_campaign_by_id_and_response_not_found_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				HttpResponseMessage response = await server.CreateClient()
                    .GetAsync(Get.CampaignBy(int.MaxValue));

                Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task Post_add_new_campaign_and_response_ok_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				CampaignDTO fakeCampaignDto = GetFakeCampaignDto();
				StringContent content = new StringContent(JsonConvert.SerializeObject(fakeCampaignDto), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await server.CreateClient()
                    .PostAsync(Post.AddNewCampaign, content);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Delete_delete_campaign_and_response_not_content_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				CampaignDTO fakeCampaignDto = GetFakeCampaignDto();
				StringContent content = new StringContent(JsonConvert.SerializeObject(fakeCampaignDto), Encoding.UTF8, "application/json");

				//add campaign
				HttpResponseMessage campaignResponse = await server.CreateClient()
                    .PostAsync(Post.AddNewCampaign, content);

                if (int.TryParse(campaignResponse.Headers.Location.Segments[4], out int id))
                {
					HttpResponseMessage response = await server.CreateClient()
                    .DeleteAsync(Delete.CampaignBy(id));

                    Assert.True(response.StatusCode == HttpStatusCode.NoContent);
                }

                campaignResponse.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Put_update_campaign_and_response_not_content_status_code()
        {
            using (Microsoft.AspNetCore.TestHost.TestServer server = CreateServer())
            {
				CampaignDTO fakeCampaignDto = GetFakeCampaignDto();
				StringContent content = new StringContent(JsonConvert.SerializeObject(fakeCampaignDto), Encoding.UTF8, "application/json");

				//add campaign
				HttpResponseMessage campaignResponse = await server.CreateClient()
                    .PostAsync(Post.AddNewCampaign, content);

                if (int.TryParse(campaignResponse.Headers.Location.Segments[4], out int id))
                {
                    fakeCampaignDto.Description = "FakeCampaignUpdatedDescription";
                    content = new StringContent(JsonConvert.SerializeObject(fakeCampaignDto), Encoding.UTF8, "application/json");
					HttpResponseMessage response = await server.CreateClient()
                    .PutAsync(Put.CampaignBy(id), content);

                    Assert.True(response.StatusCode == HttpStatusCode.Created);
                }

                campaignResponse.EnsureSuccessStatusCode();
            }
        }

        private static CampaignDTO GetFakeCampaignDto()
        {
            return new CampaignDTO()
            {
                Name = "FakeCampaignName",
                Description = "FakeCampaignDescription",
                From = DateTime.Now,
                To = DateTime.Now.AddDays(7),
                PictureUri = "http://externalcatalogbaseurltobereplaced/api/v1/campaigns/0/pic"
            }; 
        }
    }
}

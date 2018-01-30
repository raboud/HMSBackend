namespace HMS.UnitTests.Services
{
    using System.Threading.Tasks;
    using Core;
    using Core.Helpers;
    using Core.Services.Marketing;
    using Xunit;

    public class MarketingServiceTests
    {
        [Fact]
        public async Task GetFakeCampaigTest()
        {
			CampaignMockService campaignMockService = new CampaignMockService();
			Core.Models.Marketing.CampaignItem order = await campaignMockService.GetCampaignByIdAsync(1, GlobalSetting.Instance.AuthToken);

            Assert.NotNull(order);
        }

        [Fact]
        public async Task GetFakeCampaignsTest()
        {
			CampaignMockService campaignMockService = new CampaignMockService();
			System.Collections.ObjectModel.ObservableCollection<Core.Models.Marketing.CampaignItem> result = await campaignMockService.GetAllCampaignsAsync(GlobalSetting.Instance.AuthToken);

            Assert.NotEmpty(result);
        }
    }
}
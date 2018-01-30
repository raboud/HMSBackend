using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HMS.Core.Models.Marketing;
using HMS.Core.Services.RequestProvider;
using HMS.Core.Extensions;
using HMS.Core.Helpers;

namespace HMS.Core.Services.Marketing
{
    public class CampaignService : ICampaignService
    {
        private readonly IRequestProvider _requestProvider;

        public CampaignService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ObservableCollection<CampaignItem>> GetAllCampaignsAsync(string token)
        {
            UriBuilder builder = new UriBuilder(GlobalSetting.Instance.MarketingEndpoint);

            builder.Path = "api/v1/campaigns/user";

            string uri = builder.ToString();

            CampaignRoot campaign =
                await _requestProvider.GetAsync<CampaignRoot>(uri, token);

            if (campaign?.Data != null)
            {
                ServicesHelper.FixCampaignItemPictureUri(campaign?.Data);

                return campaign?.Data.ToObservableCollection();
            }

            return new ObservableCollection<CampaignItem>();
        }

        public async Task<CampaignItem> GetCampaignByIdAsync(int campaignId, string token)
        {
            UriBuilder builder = new UriBuilder(GlobalSetting.Instance.MarketingEndpoint);

            builder.Path = $"api/v1/campaigns/{campaignId}";

            string uri = builder.ToString();

            return await _requestProvider.GetAsync<CampaignItem>(uri, token);
        }
    }
}
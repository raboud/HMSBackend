using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using HMS.Core.Models.Marketing;
using HMS.Core.Services.Marketing;
using HMS.Core.ViewModels.Base;
using HMS.Core.Helpers;

namespace HMS.Core.ViewModels
{
    public class CampaignViewModel : ViewModelBase
    {
        private ObservableCollection<CampaignItem> _campaigns;
        private readonly ICampaignService _campaignService;

        public CampaignViewModel(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public ObservableCollection<CampaignItem> Campaigns
        {
            get => _campaigns;
            set
            {
                _campaigns = value;
                RaisePropertyChanged(() => Campaigns);
            }
        }

        public ICommand GetCampaignDetailsCommand => new Command<CampaignItem>(async (item) => await GetCampaignDetailsAsync(item));

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            // Get campaigns by user
            Campaigns = await _campaignService.GetAllCampaignsAsync(Settings.AuthAccessToken);

            IsBusy = false;
        }

        private async Task GetCampaignDetailsAsync(CampaignItem campaign)
        {
            await NavigationService.NavigateToAsync<CampaignDetailsViewModel>(campaign.Id);
        }
    }
}
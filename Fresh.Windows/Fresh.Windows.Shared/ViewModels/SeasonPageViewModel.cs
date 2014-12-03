using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;

namespace Fresh.Windows.ViewModels
{
    public class SeasonPageViewModel : ViewModel, ISeasonPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly ICrawlerService crawlerService;
        private readonly IStorageService storageService;

        public SeasonPageViewModel(ITraktService traktService, ICrawlerService crawlerService, IStorageService storageService)
        {
            this.traktService = traktService;
            this.crawlerService = crawlerService;
            this.storageService = storageService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var seasonId = (int)navigationParameter;

            var season = await storageService.GetSeasonAsync(seasonId);

            Number = season.Number;
            Episodes = new ObservableCollection<Episode>(season.Episodes);
        }

        public DelegateCommand<Episode> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<Episode>(EpisodeSelected);
            }
        }

        private async void EpisodeSelected(Episode episode)
        {
            if (episode.Link == null)
            {
                episode.Link = await crawlerService.GetLink(episode.Season.TVShow.Title, episode.Season.Number, episode.Number);

                await storageService.UpdateEpisodeAsync(episode);
            }
        }

        ObservableCollection<Episode> episodes = default(ObservableCollection<Episode>);
        public ObservableCollection<Episode> Episodes { get { return episodes; } set { SetProperty(ref episodes, value); } }

        int number = default(int);
        public int Number { get { return number; } set { SetProperty(ref number, value); } }
    }
}

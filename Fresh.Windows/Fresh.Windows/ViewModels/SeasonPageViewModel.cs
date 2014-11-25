using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Interfaces;
using Fresh.Windows.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;

namespace Fresh.Windows.ViewModels
{
    public class SeasonPageViewModel : ViewModel, ISeasonPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly ICrawlerService crawlerService;

        private string showTitle;

        public SeasonPageViewModel(ITraktService traktService, ICrawlerService crawlerService)
        {
            this.traktService = traktService;
            this.crawlerService = crawlerService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            dynamic parameters = navigationParameter;
            string showId = parameters.showId;
            var seasonNumber = (int)parameters.seasonNumber;
            showTitle = (string)parameters.showTitle;

            Number = seasonNumber; 

            var episodes = (await traktService.GetSeason(showId, seasonNumber, extended: true)).Select(Episode.FromTrakt).ToList();
            Episodes = new ObservableCollection<IEpisodeViewModel>(episodes.Select(e => new EpisodeViewModel
            {
                Number = e.Number,
                Season = e.Season,
                Screen = e.Screen,
                Overview = e.Overview,
                FirstAired = e.FirstAired,
                Links = new ObservableCollection<string>(e.Links ?? Enumerable.Empty<string>())
            }));
        }

        public DelegateCommand<IEpisodeViewModel> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<IEpisodeViewModel>(EpisodeSelected);
            }
        }

        private async void EpisodeSelected(IEpisodeViewModel episode)
        {
            episode.Links = new ObservableCollection<string>(await crawlerService.GetLinks(showTitle, episode.Season, episode.Number));
        }

        ObservableCollection<IEpisodeViewModel> episodes = default(ObservableCollection<IEpisodeViewModel>);
        public ObservableCollection<IEpisodeViewModel> Episodes { get { return episodes; } set { SetProperty(ref episodes, value); } }

        int number = default(int);
        public int Number { get { return number; } set { SetProperty(ref number, value); } }
    }
}

using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Fresh.Windows.Core.Models;

namespace Fresh.Windows.ViewModels
{
    public class SeasonPageViewModel : ViewModel, ISeasonPageViewModel
    {
        public INavigationService navigationService { get; private set; }
        private readonly ITraktService traktService;

        public TraktSeason Season { get; private set; }
        private int showId;

        public SeasonPageViewModel(INavigationService navigationService, ITraktService traktService)
        {
            this.navigationService = navigationService;
            this.traktService = traktService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            dynamic parameters = navigationParameter;
            var seasonNumber = (int)parameters.season;
            showId = (int)parameters.showId;

            var traktSeasons = await traktService.GetSeasonsAsync(showId, extended: TraktExtendEnum.FULL_IMAGES);
            Season = (from traktseason in traktSeasons
                      where traktseason.Number == seasonNumber
                      select traktseason).Single();

            Number = Season.Number;
            Poster = Season.Images.Poster.Full;
            Overview = Season.Overview;
            Episodes = new ObservableCollection<TraktEpisode>(from episode in await traktService.GetSeasonEpisodesAsync(showId, seasonNumber, extended: TraktExtendEnum.IMAGES)
                                                              orderby episode.Number
                                                              select episode);
        }

        public DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(args => EpisodeSelected((TraktEpisode)args.ClickedItem));
            }
        }

        private void EpisodeSelected(TraktEpisode episode)
        {
            navigationService.Navigate(App.Experience.Episode.ToString(),
                new { showId = showId, season = episode.Season, episode = episode.Number });
        }

        public DelegateCommand SeasonWatchedCommand
        {
            get
            {
                return new DelegateCommand(SeasonWatched);
            }
        }

        private async void SeasonWatched()
        {
            if (Episodes.Count == 0)
                return;

            await traktService.WatchEpisodesAsync((from episode in episodes
                                                   select episode.Ids.Tvdb ?? episode.Ids.Trakt).ToList());
        }

        ObservableCollection<TraktEpisode> episodes = default(ObservableCollection<TraktEpisode>);
        public ObservableCollection<TraktEpisode> Episodes { get { return episodes; } set { SetProperty(ref episodes, value); } }

        int number = default(int);
        public int Number { get { return number; } set { SetProperty(ref number, value); } }

        string poster = default(string);
        public string Poster { get { return poster; } set { SetProperty(ref poster, value); } }

        string overview = default(string);
        public string Overview { get { return overview; } set { SetProperty(ref overview, value); } }
    }
}
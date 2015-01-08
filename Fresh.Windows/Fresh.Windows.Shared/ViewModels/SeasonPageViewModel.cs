using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace Fresh.Windows.ViewModels
{
    public class SeasonPageViewModel : ViewModel, ISeasonPageViewModel
    {
        private readonly IStorageService storageService;
        public INavigationService navigationService { get; private set; }
        private readonly ITraktService traktService;

        public Season Season { get; private set; }

        private bool isStored;

        public SeasonPageViewModel(IStorageService storageService, INavigationService navigationService, ITraktService traktService)
        {
            this.storageService = storageService;
            this.navigationService = navigationService;
            this.traktService = traktService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            dynamic parameters = navigationParameter;
            var seasonNumber = (int)parameters.season;
            var showId = (int)parameters.showId;

            Season = await storageService.GetSeasonAsync(showId, seasonNumber);

            isStored = Season != null;

            if (!isStored)
            {
                var traktSeasons = await traktService.GetSeasonsAsync(showId, extended: TraktExtendEnum.FULL_IMAGES);
                Season = (from traktseason in traktSeasons
                          where traktseason.Number == seasonNumber
                          select Season.FromTrakt(traktseason)).Single();
                Season.TVShowId = showId;
                await Season.UpdateAsync(traktService);
            }

            Number = seasonNumber;
            Poster = Season.Poster;
            Overview = Season.Overview;
            Episodes = new ObservableCollection<Episode>(from episode in Season.Episodes
                                                         orderby episode.Number
                                                         select episode);
        }

        public DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(args => EpisodeSelected((Episode)args.ClickedItem));
            }
        }

        private void EpisodeSelected(Episode episode)
        {
            navigationService.Navigate(App.Experience.Episode.ToString(),
                new { showId = Season.TVShowId, season = Season.Number, episode = episode.Number, episodeId = episode.Id });
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
            var episodes = (from episode in Episodes
                            where episode.Watched == false
                            select episode).ToList();

            if (episodes.Count == 0)
                return;

            await traktService.WatchEpisodesAsync((from episode in episodes
                                                   select episode.Id).ToList());

            foreach (var episode in episodes)
            {
                episode.Watched = true;
                if (isStored)
                    await storageService.UpdateEpisodeAsync(episode);
            }
        }

        ObservableCollection<Episode> episodes = default(ObservableCollection<Episode>);
        public ObservableCollection<Episode> Episodes { get { return episodes; } set { SetProperty(ref episodes, value); } }

        int number = default(int);
        public int Number { get { return number; } set { SetProperty(ref number, value); } }

        string poster = default(string);
        public string Poster { get { return poster; } set { SetProperty(ref poster, value); } }

        string overview = default(string);
        public string Overview { get { return overview; } set { SetProperty(ref overview, value); } }
    }
}

using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using Windows.UI.Xaml.Controls;
using System.Linq;
using System;
using Fresh.Windows.Core.Services.Interfaces;

namespace Fresh.Windows.ViewModels
{
    public class TVShowPageViewModel : ViewModel, ITVShowPageViewModel
    {
        public INavigationService navigationService { get; private set; }
        private readonly IStorageService storageService;
        private readonly ITraktService traktService;

        private bool isStored;

        public TVShow Show { get; private set; }

        public TVShowPageViewModel(INavigationService navigationService, IStorageService storageService, ITraktService traktService)
        {
            this.navigationService = navigationService;
            this.storageService = storageService;
            this.traktService = traktService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var showId = (int)navigationParameter;

            Show = await storageService.GetShowAsync(showId);

            isStored = Show != null;

            if (!isStored)
            {
                Show = TVShow.FromTrakt(await traktService.GetShowAsync(showId, extended: TraktExtendEnum.FULL_IMAGES));
                await Show.UpdateAsync(traktService);
            }

            Update();
        }

        public DelegateCommand<ItemClickEventArgs> EnterSeasonCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(arg => EnterSeason(((Season)arg.ClickedItem).Number));
            }
        }

        private void EnterSeason(int season)
        {
            navigationService.Navigate(App.Experience.Season.ToString(), new { season, showId = Show.Id });
        }

        private void Update()
        {
            Title = Show.Title;
            Poster = Show.Poster;
            Overview = Show.Overview;
            Rating = Show.Rating;

            Seasons = new ObservableCollection<Season>(from season in Show.Seasons
                                                       orderby season.Number descending
                                                       select season);

            if (isStored)
                UnwatchedEpisodes = new ObservableCollection<Episode>(from season in Show.Seasons
                                                                      from episode in season.Episodes
                                                                      where episode.Watched == false &&
                                                                        episode.AirDate.HasValue &&
                                                                        episode.AirDate <= DateTime.UtcNow &&
                                                                        season.Number != 0
                                                                      orderby season.Number, episode.Number
                                                                      select episode);
        }

        public DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(args =>
                    {
                        var episode = (Episode)args.ClickedItem;
                        navigationService.Navigate(App.Experience.Episode.ToString(),
                new { showId = Show.Id, season = episode.Season.Number, episode = episode.Number, episodeId = episode.Id });
                    });
            }
        }

        private void EpisodeSelected(Episode episode)
        {
            navigationService.Navigate(App.Experience.Episode.ToString(), episode.Id);
        }

        string title = default(string);
        public string Title { get { return title; } set { SetProperty(ref title, value); } }

        string poster = default(string);
        public string Poster { get { return poster; } set { SetProperty(ref poster, value); } }

        string overview = default(string);
        public string Overview { get { return overview; } set { SetProperty(ref overview, value); } }

        double rating = default(double);
        public double Rating { get { return rating; } set { SetProperty(ref rating, value); } }

        ObservableCollection<Season> seasons = default(ObservableCollection<Season>);
        public ObservableCollection<Season> Seasons { get { return seasons; } set { SetProperty(ref seasons, value); } }

        ObservableCollection<Episode> unwatchedEpisodes = new ObservableCollection<Episode>(Enumerable.Empty<Episode>());
        public ObservableCollection<Episode> UnwatchedEpisodes { get { return unwatchedEpisodes; } set { SetProperty(ref unwatchedEpisodes, value); } }
    }
}

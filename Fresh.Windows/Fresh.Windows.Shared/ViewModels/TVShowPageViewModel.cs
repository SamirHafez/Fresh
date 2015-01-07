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

namespace Fresh.Windows.ViewModels
{
    public class TVShowPageViewModel : ViewModel, ITVShowPageViewModel
    {
        public INavigationService navigationService { get; private set; }
        private readonly IStorageService storageService;

        private int showId;

        public TVShowPageViewModel(INavigationService navigationService, IStorageService storageService)
        {
            this.navigationService = navigationService;
            this.storageService = storageService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            showId = (int)navigationParameter;

            var dbShow = await storageService.GetShowAsync(showId);

            Update(dbShow);
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
            navigationService.Navigate(App.Experience.Season.ToString(), new { season, showId });
        }

        private void Update(TVShow fullShow)
        {
            Title = fullShow.Title;
            Poster = fullShow.Poster;
            Overview = fullShow.Overview;
            Rating = fullShow.Rating;

            Seasons = new ObservableCollection<Season>(from season in fullShow.Seasons
                                                       orderby season.Number descending
                                                       select season);

            UnwatchedEpisodes = new ObservableCollection<Episode>(from season in fullShow.Seasons
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
                    navigationService.Navigate(App.Experience.Episode.ToString(), ((Episode)args.ClickedItem).Id));
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

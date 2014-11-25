using Fresh.Windows.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Models;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace Fresh.Windows.ViewModels
{
    public class TVShowPageViewModel : ViewModel, ITVShowPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly INavigationService navigationService;

        private string showId;
        private string showTitle;

        private const int TOP_EPISODE_COUNT = 3;

        public TVShowPageViewModel(ITraktService traktService, INavigationService navigationService)
        {
            this.traktService = traktService;
            this.navigationService = navigationService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            showId = navigationParameter as string;

            var fullShow = TVShow.FromTrakt(await traktService.GetShow(showId, extended: true));

            showTitle = fullShow.Title;

            Update(fullShow);
        }

        public DelegateCommand<Season> EnterSeasonCommand
        {
            get
            {
                return new DelegateCommand<Season>(EnterSeason);
            }
        }

        private void EnterSeason(Season season)
        {
            navigationService.Navigate(App.Experience.Season.ToString(), new { showId, seasonNumber = season.Number, showTitle });
        }

        private void Update(TVShow fullShow)
        {
            Title = fullShow.Title;
            Year = fullShow.Year;
            Network = fullShow.Network;
            Overview = fullShow.Overview;
            Images = fullShow.Images;
            FirstAired = fullShow.FirstAired;
            Seasons = new ObservableCollection<Season>(fullShow.Seasons);
            TopEpisodes = new ObservableCollection<Episode>(Seasons.SelectMany(s => s.Episodes).OrderByDescending(e => e.Plays).Take(TOP_EPISODE_COUNT));
            Actors = new ObservableCollection<Actor>(fullShow.Actors);
            Ratings = fullShow.Ratings;
            Stats = fullShow.Stats;
        }

        string title = default(string);
        public string Title { get { return title; } set { SetProperty(ref title, value); } }

        int year = default(int);
        public int Year { get { return year; } set { SetProperty(ref year, value); } }

        string url = default(string);
        public string Url { get { return url; } set { SetProperty(ref url, value); } }

        DateTime firstAired = default(DateTime);
        public DateTime FirstAired { get { return firstAired; } set { SetProperty(ref firstAired, value); } }

        string country = default(string);
        public string Country { get { return country; } set { SetProperty(ref country, value); } }

        string overview = default(string);
        public string Overview { get { return overview; } set { SetProperty(ref overview, value); } }

        TimeSpan runtime = default(TimeSpan);
        public TimeSpan Runtime { get { return runtime; } set { SetProperty(ref runtime, value); } }

        string status = default(string);
        public string Status { get { return status; } set { SetProperty(ref status, value); } }

        string network = default(string);
        public string Network { get { return network; } set { SetProperty(ref network, value); } }

        DayOfWeek airDay = default(DayOfWeek);
        public DayOfWeek AirDay { get { return airDay; } set { SetProperty(ref airDay, value); } }

        TimeSpan airTime = default(TimeSpan);
        public TimeSpan AirTime { get { return airTime; } set { SetProperty(ref airTime, value); } }

        DateTime lastUpdate = default(DateTime);
        public DateTime LastUpdate { get { return lastUpdate; } set { SetProperty(ref lastUpdate, value); } }

        Images images = default(Images);
        public Images Images { get { return images; } set { SetProperty(ref images, value); } }

        Ratings ratings = default(Ratings);
        public Ratings Ratings { get { return ratings; } set { SetProperty(ref ratings, value); } }

        Stats stats = default(Stats);
        public Stats Stats { get { return stats; } set { SetProperty(ref stats, value); } }

        ObservableCollection<Actor> actors = default(ObservableCollection<Actor>);
        public ObservableCollection<Actor> Actors { get { return actors; } set { SetProperty(ref actors, value); } }

        ObservableCollection<Episode> topEpisodes = default(ObservableCollection<Episode>);
        public ObservableCollection<Episode> TopEpisodes { get { return topEpisodes; } set { SetProperty(ref topEpisodes, value); } }

        ObservableCollection<Season> seasons = default(ObservableCollection<Season>);
        public ObservableCollection<Season> Seasons { get { return seasons; } set { SetProperty(ref seasons, value); } }

        ObservableCollection<string> genres = default(ObservableCollection<string>);
        public ObservableCollection<string> Genres { get { return genres; } set { SetProperty(ref genres, value); } } 
    }
}

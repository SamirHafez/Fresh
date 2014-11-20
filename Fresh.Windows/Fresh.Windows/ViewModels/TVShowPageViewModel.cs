using Fresh.Windows.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Fresh.Windows.Core.Services.Interfaces;

namespace Fresh.Windows.ViewModels
{
    public class TVShowPageViewModel : ViewModel, ITVShowPageViewModel
    {
        private readonly ITraktService traktService;

        public TVShowPageViewModel(ITraktService traktService)
        {
            this.traktService = traktService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var showId = navigationParameter as string;

            var fullShow = await traktService.GetShow(showId);
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

        string imdbId = default(string);
        public string ImdbId { get { return imdbId; } set { SetProperty(ref imdbId, value); } }

        int tvdbId = default(int);
        public int TvdbId { get { return tvdbId; } set { SetProperty(ref tvdbId, value); } }

        int tvrageId = default(int);
        public int TvrageId { get { return tvrageId; } set { SetProperty(ref tvrageId, value); } }

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

        ObservableCollection<TopWatcher> top_watchers = default(ObservableCollection<TopWatcher>);
        public ObservableCollection<TopWatcher> Top_watchers { get { return top_watchers; } set { SetProperty(ref top_watchers, value); } }

        ObservableCollection<IEpisodePageViewModel> episodes = default(ObservableCollection<IEpisodePageViewModel>);
        public ObservableCollection<IEpisodePageViewModel> Episodes { get { return episodes; } set { SetProperty(ref episodes, value); } }

        ObservableCollection<string> genres = default(ObservableCollection<string>);
        public ObservableCollection<string> Genres { get { return genres; } set { SetProperty(ref genres, value); } } 
    }
}

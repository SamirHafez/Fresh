using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Configuration;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.UI.Xaml.Data;
using Fresh.Windows.Core.Models;
using System.Threading.Tasks;

namespace Fresh.Windows.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly IStorageService storageService;
        private readonly INavigationService navigationService;
        private readonly ISession configurationService;

        public MainPageViewModel(ITraktService traktService, IStorageService storageService, INavigationService navigationService, ISession configurationService)
        {
            this.traktService = traktService;
            this.storageService = storageService;
            this.navigationService = navigationService;
            this.configurationService = configurationService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var username = configurationService.User.Username;

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username not provided.");

            Library = new ObservableCollection<TVShow>(from show in await storageService.GetLibraryAsync()
                                                       orderby show.Title
                                                       select show);

            if (Library.Count == 0)
                Library = new ObservableCollection<TVShow>(await FirstLoadAsync(username));

            var lastMonday = StartOfWeek(DateTime.UtcNow, DayOfWeek.Monday);
            var nextSunday = lastMonday.AddDays(7);
            ThisWeek = from episode in await storageService.GetEpisodesAsync(e => e.AirDate >= lastMonday && e.AirDate < nextSunday)
                       group episode by episode.Season.TVShow.AirDay into g
                       where g.Key.HasValue
                       orderby g.Key
                       select new GroupedEpisodes<DayOfWeek>
                       {
                           Key = g.Key.Value,
                           Episodes = g.ToList()
                       };
        }

        private async Task<IList<TVShow>> FirstLoadAsync(string username)
        {
            var traktFullShowTasks = from show in await traktService.GetLibraryAsync(username, extended: false)
                                     let id = TVShow.FromTrakt(show).Id
                                     select traktService.GetShowAsync(id, extended: true);

            var fullShows = (from show in await Task.WhenAll(traktFullShowTasks)
                             orderby show.Title
                             select TVShow.FromTrakt(show)).ToList();

            var watchedEpisodes = await traktService.GetWatchedEpisodesAsync(username);

            foreach (var watchedEpisode in watchedEpisodes)
            {
                var show = fullShows.First(s => s.Title == watchedEpisode.Show.Title);

                var episode = (from s in show.Seasons
                               from ep in s.Episodes
                               where s.Number == watchedEpisode.Episode.Season && ep.Number == watchedEpisode.Episode.Number
                               select ep).First();

                episode.Watched = true;
            }

            await storageService.UpdateLibraryAsync(fullShows);

            return fullShows;
        }

        public DelegateCommand<Episode> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<Episode>(EpisodeSelected);
            }
        }

        private void EpisodeSelected(Episode episode)
        {
            navigationService.Navigate(App.Experience.Episode.ToString(), episode.Id);
        }

        public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff);
        }

        ObservableCollection<TVShow> library = default(ObservableCollection<TVShow>);
        public ObservableCollection<TVShow> Library { get { return library; } set { SetProperty(ref library, value); } }

        IEnumerable<GroupedEpisodes<DayOfWeek>> thisWeek = default(IEnumerable<GroupedEpisodes<DayOfWeek>>);
        public IEnumerable<GroupedEpisodes<DayOfWeek>> ThisWeek { get { return thisWeek; } set { SetProperty(ref thisWeek, value); } }

        public DelegateCommand<TVShow> EnterShowCommand
        {
            get
            {
                return new DelegateCommand<TVShow>(EnterShow);
            }
        }

        private void EnterShow(TVShow show)
        {
            navigationService.Navigate(App.Experience.TVShow.ToString(), show.Id);
        }
    }
}

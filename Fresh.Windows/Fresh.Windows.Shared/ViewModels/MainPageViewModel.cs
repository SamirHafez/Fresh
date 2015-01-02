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
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Fresh.Windows.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly IStorageService storageService;
        private readonly INavigationService navigationService;
        private readonly ISession session;

        public MainPageViewModel(ITraktService traktService, IStorageService storageService, INavigationService navigationService, ISession session)
        {
            this.traktService = traktService;
            this.storageService = storageService;
            this.navigationService = navigationService;
            this.session = session;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var username = session.User.Username;

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username not provided.");

            Loading = true;

            Library = new ObservableCollection<TVShow>(from show in await storageService.GetLibraryAsync()
                                                       orderby show.Title
                                                       select show);

            if (navigationMode == NavigationMode.New)
                try
                {
                    var updateTasks = from show in Library
                                      select show.UpdateAsync(traktService);

                    var watchedShows = await traktService.GetWatchedEpisodesAsync(extended: TraktExtendEnum.MIN);

                    await Task.WhenAll(updateTasks);

                    foreach (var watchedShow in watchedShows)
                    {
                        var show = Library.FirstOrDefault(s => s.Id == watchedShow.Show.Ids.Trakt);

                        if (show == null)
                        {
                            show = TVShow.FromTrakt(await traktService.GetShowAsync(watchedShow.Show.Ids.Trakt, extended: TraktExtendEnum.FULL_IMAGES));
                        }

                        show.UpdateWatched(watchedShow);
                    }

                    await storageService.UpdateLibraryAsync(Library);
                }
                catch
                { }

            //var lastMonday = StartOfWeek(DateTime.Now, DayOfWeek.Monday);
            //var nextSunday = lastMonday.AddDays(7);
            //ThisWeek = await GetSchedule(lastMonday, nextSunday);

            //UnwatchedEpisodesByShow = new ObservableCollection<GroupedEpisodes<TVShow>>(await GetUnwatchedEpisodesByShow());

            Loading = false;

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        private async Task<IEnumerable<GroupedEpisodes<TVShow>>> GetUnwatchedEpisodesByShow()
        {
            return from episode in await storageService.GetEpisodesAsync(e => e.Watched == false && e.AirDate != null && e.AirDate <= DateTime.UtcNow && e.Season.Number > 0)
                   group episode by episode.Season.TVShowId into g
                   let tvShow = g.First().Season.TVShow
                   orderby g.Count() descending
                   select new GroupedEpisodes<TVShow>
                   {
                       Key = tvShow,
                       Episodes = g.ToList()
                   };
        }

        private async Task<IList<GroupedEpisodes<DayOfWeek>>> GetSchedule(DateTime lastMonday, DateTime nextSunday)
        {
            var utcLastMonday = lastMonday.ToUniversalTime();
            var utcNextSunday = nextSunday.ToUniversalTime();

            return (from episode in await storageService.GetEpisodesAsync(e => e.AirDate != null && e.AirDate >= utcLastMonday && e.AirDate <= utcNextSunday)
                    group episode by episode.AirDate.Value.ToLocalTime().DayOfWeek into g
                    orderby g.Key
                    select new GroupedEpisodes<DayOfWeek>
                    {
                        Key = g.Key,
                        Episodes = g.ToList()
                    }).ToList();
        }

        public DelegateCommand<string> EnterSearchCommand
        {
            get
            {
                return new DelegateCommand<string>(query =>
                    navigationService.Navigate(App.Experience.SearchResults.ToString(), query));
            }
        }

        public DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(args =>
                    navigationService.Navigate(App.Experience.Episode.ToString(), ((Episode)args.ClickedItem).Id));
            }
        }

        public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        ObservableCollection<TVShow> library = default(ObservableCollection<TVShow>);
        public ObservableCollection<TVShow> Library { get { return library; } set { SetProperty(ref library, value); } }

        ObservableCollection<GroupedEpisodes<TVShow>> unwatchedEpisodeByShow = new ObservableCollection<GroupedEpisodes<TVShow>>(Enumerable.Empty<GroupedEpisodes<TVShow>>());
        public ObservableCollection<GroupedEpisodes<TVShow>> UnwatchedEpisodesByShow { get { return unwatchedEpisodeByShow; } set { SetProperty(ref unwatchedEpisodeByShow, value); } }

        IList<GroupedEpisodes<DayOfWeek>> thisWeek = default(IList<GroupedEpisodes<DayOfWeek>>);
        public IList<GroupedEpisodes<DayOfWeek>> ThisWeek { get { return thisWeek; } set { SetProperty(ref thisWeek, value); } }

        bool loading = default(bool);
        public bool Loading { get { return loading; } set { SetProperty(ref loading, value); } }

        public DelegateCommand<ItemClickEventArgs> EnterShowCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(args =>
                {
                    var tvShow = args.ClickedItem is TVShow ?
                        (TVShow)args.ClickedItem :
                        ((GroupedEpisodes<TVShow>)args.ClickedItem).Key;

                    navigationService.Navigate(App.Experience.TVShow.ToString(), tvShow.Id);
                });
            }
        }
    }
}

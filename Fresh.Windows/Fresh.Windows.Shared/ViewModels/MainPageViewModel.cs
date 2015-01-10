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
using Windows.UI.Xaml.Controls;
using System.Globalization;
using System.Threading.Tasks;

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
                    var updateTasks = (from show in Library
                                       select new { ShowId = show.Id, Task = show.UpdateAsync(traktService) }).ToList();

                    var lastActivity = await traktService.GetLastActivityAsync();
                    var lastActivityEpisodes = DateTime.Parse(lastActivity.Episodes.Watched_At, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

                    if (lastActivityEpisodes > (session.User.ActivityUpdated ?? DateTime.MinValue))
                    {
                        var watchedShows = await traktService.GetWatchedEpisodesAsync(extended: TraktExtendEnum.MIN);

                        foreach (var watchedShow in watchedShows)
                        {
                            var show = Library.FirstOrDefault(s => s.Id == watchedShow.Show.Ids.Trakt);

                            bool isNew = show == null;
                            if (isNew)
                            {
                                show = TVShow.FromTrakt(await traktService.GetShowAsync(watchedShow.Show.Ids.Trakt, extended: TraktExtendEnum.FULL_IMAGES));
                                await show.UpdateAsync(traktService);
                            }
                            else
                            {
                                var task = (from updateTask in updateTasks
                                            where updateTask.ShowId == show.Id
                                            select updateTask.Task).First();
                                await task;
                            }

                            show.UpdateWatched(watchedShow);

                            if (isNew)
                                Library.Add(show);
                            await storageService.UpdateShowAsync(show);
                        }

                        session.User.ActivityUpdated = lastActivityEpisodes;
                        await storageService.CreateOrUpdateUserAsync(session.User);
                    }
                    else
                        await Task.WhenAll(updateTasks.Select(ut => ut.Task));
                }
                catch
                { }

            var lastMonday = StartOfWeek(DateTime.Now, DayOfWeek.Monday);
            var nextSunday = lastMonday.AddDays(7);
            ThisWeek = GetSchedule(lastMonday, nextSunday).ToList();

            UnwatchedEpisodesByShow = new ObservableCollection<GroupedEpisodes<TVShow>>(GetUnwatchedEpisodesByShow());

            Loading = false;

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        private IEnumerable<GroupedEpisodes<TVShow>> GetUnwatchedEpisodesByShow()
        {
            return from show in Library
                   from season in show.Seasons
                   from episode in season.Episodes
                   where episode.Watched == false && episode.AirDate != null && episode.AirDate <= DateTime.UtcNow && season.Number > 0
                   group episode by episode.Season.TVShowId into g
                   let tvShow = g.First().Season.TVShow
                   orderby g.Count()
                   select new GroupedEpisodes<TVShow>
                   {
                       Key = tvShow,
                       Episodes = g.ToList()
                   };
        }

        private IEnumerable<GroupedEpisodes<DayOfWeek>> GetSchedule(DateTime lastMonday, DateTime nextSunday)
        {
            var utcLastMonday = lastMonday.ToUniversalTime();
            var utcNextSunday = nextSunday.ToUniversalTime();

            return from show in Library
                   from season in show.Seasons
                   from episode in season.Episodes
                   where episode.AirDate != null && episode.AirDate >= utcLastMonday && episode.AirDate <= utcNextSunday
                   group episode by episode.AirDate.Value.ToLocalTime().DayOfWeek into g
                   orderby g.Key
                   select new GroupedEpisodes<DayOfWeek>
                   {
                       Key = g.Key,
                       Episodes = g.ToList()
                   };
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

                return new DelegateCommand<ItemClickEventArgs>(arg =>
                    {
                        var episode = (Episode)arg.ClickedItem;
                        navigationService.Navigate(App.Experience.Episode.ToString(),
                            new { showId = episode.Season.TVShowId, season = episode.Season.Number, episode = episode.Number, episodeId = episode.Id });
                    });
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

        ObservableCollection<TVShow> library = new ObservableCollection<TVShow>();
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

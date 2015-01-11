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
using Fresh.Windows.Core.Models;

namespace Fresh.Windows.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly INavigationService navigationService;
        private readonly ISession session;

        public MainPageViewModel(ITraktService traktService, INavigationService navigationService, ISession session)
        {
            this.traktService = traktService;
            this.navigationService = navigationService;
            this.session = session;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var username = session.User.Username;

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username not provided.");

            Loading = true;

            await Task.WhenAll(
                FetchCalendarAsync(),
                FetchNextEpisodesAsync(),
                FetchRecommendedShowsAsync(),
                FetchPopularShowsAsync(),
                FetchTrendingShowsAsync()
                );

            Loading = false;

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        private async Task FetchTrendingShowsAsync()
        {
            Trending = new ObservableCollection<TVShow>(from item in await traktService.GetTrendingShowsAsync(extended: TraktExtendEnum.IMAGES, limit: 5)
                                                        orderby item.Watchers descending
                                                        select TVShow.FromTrakt(item.Show));
        }

        private async Task FetchPopularShowsAsync()
        {
            Popular = new ObservableCollection<TVShow>(from show in await traktService.GetPopularShowsAsync(extended: TraktExtendEnum.IMAGES, limit: 5)
                                                       select TVShow.FromTrakt(show));
        }

        private async Task FetchRecommendedShowsAsync()
        {
            Recommended = new ObservableCollection<TVShow>(from show in await traktService.GetRecommendedShowsAsync(extended: TraktExtendEnum.IMAGES)
                                                           select TVShow.FromTrakt(show));
        }

        private async Task FetchCalendarAsync()
        {
            var days = 7;
            var startDate = StartOfWeek(DateTime.Today, DayOfWeek.Monday);
            var endDate = startDate.AddDays(days);
            ThisWeek = (from day in await traktService.GetCalendarAsync(startDate.ToUniversalTime().AddDays(-1), days + 1, extended: TraktExtendEnum.IMAGES)
                        from item in day.Value
                        let airDate = DateTime.Parse(item.Airs_At, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)
                        where airDate >= startDate && airDate <= endDate
                        let episode = Episode.FromTrakt(item.Episode, item.Show.Ids.Trakt)
                        group episode by airDate.DayOfWeek into groupItem
                        orderby groupItem.Key
                        select new GroupedEpisodes<DayOfWeek>
                        {
                            Key = groupItem.Key,
                            Episodes = groupItem.ToList()
                        }).ToList();
        }

        private async Task FetchNextEpisodesAsync()
        {
            var traktProgressTasks = (from show in await traktService.GetWatchedEpisodesAsync(extended: TraktExtendEnum.MIN)
                                      select new { show.Show, Progress = traktService.GetShowWatchedProgressAsync(show.Show.Ids.Trakt, extended: TraktExtendEnum.IMAGES) }).ToList();

            await Task.WhenAll(from task in traktProgressTasks
                               select task.Progress);

            NextEpisodes = new ObservableCollection<Episode>(from item in traktProgressTasks
                                                             where item.Progress.Result.Next_Episode != null
                                                             select Episode.FromTrakt(item.Progress.Result.Next_Episode, item.Show.Ids.Trakt));
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
                            new { showId = episode.ShowId, season = episode.SeasonNumber, episode = episode.Number });
                    });
            }
        }

        public DelegateCommand<ItemClickEventArgs> EnterShowCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(args =>
                {
                    var tvShow = (TVShow)args.ClickedItem;
                    navigationService.Navigate(App.Experience.TVShow.ToString(), tvShow.Id);
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

        ObservableCollection<TVShow> recommended = new ObservableCollection<TVShow>();
        public ObservableCollection<TVShow> Recommended { get { return recommended; } set { SetProperty(ref recommended, value); } }

        ObservableCollection<TVShow> trending = new ObservableCollection<TVShow>();
        public ObservableCollection<TVShow> Trending { get { return trending; } set { SetProperty(ref trending, value); } }

        ObservableCollection<TVShow> popular = new ObservableCollection<TVShow>();
        public ObservableCollection<TVShow> Popular { get { return popular; } set { SetProperty(ref popular, value); } }

        ObservableCollection<Episode> nextEpisodes = new ObservableCollection<Episode>(Enumerable.Empty<Episode>());
        public ObservableCollection<Episode> NextEpisodes { get { return nextEpisodes; } set { SetProperty(ref nextEpisodes, value); } }

        IList<GroupedEpisodes<DayOfWeek>> thisWeek = default(IList<GroupedEpisodes<DayOfWeek>>);
        public IList<GroupedEpisodes<DayOfWeek>> ThisWeek { get { return thisWeek; } set { SetProperty(ref thisWeek, value); } }

        bool loading = default(bool);
        public bool Loading { get { return loading; } set { SetProperty(ref loading, value); } }
    }
}

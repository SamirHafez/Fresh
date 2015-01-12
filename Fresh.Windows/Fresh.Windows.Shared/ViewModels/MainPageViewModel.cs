using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Interfaces;
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

        public MainPageViewModel(ITraktService traktService, INavigationService navigationService)
        {
            this.traktService = traktService;
            this.navigationService = navigationService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
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
            Trending = new ObservableCollection<TraktTVShow>(from item in await traktService.GetTrendingShowsAsync(extended: TraktExtendEnum.IMAGES, limit: 6)
                                                             orderby item.Watchers descending
                                                             select item.Show);
        }

        private async Task FetchPopularShowsAsync()
        {
            Popular = new ObservableCollection<TraktTVShow>(await traktService.GetPopularShowsAsync(extended: TraktExtendEnum.IMAGES, limit: 6));
        }

        private async Task FetchRecommendedShowsAsync()
        {
            Recommended = new ObservableCollection<TraktTVShow>(await traktService.GetRecommendedShowsAsync(extended: TraktExtendEnum.IMAGES));
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
                        group item.Episode by airDate.DayOfWeek into groupItem
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

            NextEpisodes = new ObservableCollection<TraktEpisode>(from item in traktProgressTasks
                                                                  where item.Progress.Result.Next_Episode != null
                                                                  select item.Progress.Result.Next_Episode);
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
                        var episode = (TraktEpisode)arg.ClickedItem;
                        navigationService.Navigate(App.Experience.Episode.ToString(),
                            new { showId = 0, season = episode.Season, episode = episode.Number });
                    });
            }
        }

        public DelegateCommand<ItemClickEventArgs> EnterShowCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(args =>
                {
                    var tvShow = (TraktTVShow)args.ClickedItem;
                    navigationService.Navigate(App.Experience.TVShow.ToString(), tvShow.Ids.Trakt);
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

        ObservableCollection<TraktTVShow> recommended = new ObservableCollection<TraktTVShow>();
        public ObservableCollection<TraktTVShow> Recommended { get { return recommended; } set { SetProperty(ref recommended, value); } }

        ObservableCollection<TraktTVShow> trending = new ObservableCollection<TraktTVShow>();
        public ObservableCollection<TraktTVShow> Trending { get { return trending; } set { SetProperty(ref trending, value); } }

        ObservableCollection<TraktTVShow> popular = new ObservableCollection<TraktTVShow>();
        public ObservableCollection<TraktTVShow> Popular { get { return popular; } set { SetProperty(ref popular, value); } }

        ObservableCollection<TraktEpisode> nextEpisodes = new ObservableCollection<TraktEpisode>(Enumerable.Empty<TraktEpisode>());
        public ObservableCollection<TraktEpisode> NextEpisodes { get { return nextEpisodes; } set { SetProperty(ref nextEpisodes, value); } }

        IList<GroupedEpisodes<DayOfWeek>> thisWeek = new List<GroupedEpisodes<DayOfWeek>>();
        public IList<GroupedEpisodes<DayOfWeek>> ThisWeek { get { return thisWeek; } set { SetProperty(ref thisWeek, value); } }

        bool loading = default(bool);
        public bool Loading { get { return loading; } set { SetProperty(ref loading, value); } }
    }
}
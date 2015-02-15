using Cirrious.MvvmCross.ViewModels;
using Fresh.Core.Models;
using Fresh.Core.Services;
using Fresh.Core.Services.Interfaces;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Fresh.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        ITraktService traktService;

        ObservableCollection<TVShow> popular = default(ObservableCollection<TVShow>);
        public ObservableCollection<TVShow> Popular
        {
            get { return popular; }
            set { popular = value; RaisePropertyChanged(() => Popular); }
        }

        public MainViewModel(ITraktService traktService)
        {
            this.traktService = traktService;
        }

        protected override async void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);

            await Task.WhenAll(
                FetchCalendarAsync()
                //FetchNextEpisodesAsync(),
                //FetchRecommendedShowsAsync(),
                //FetchPopularShowsAsync(),
                //FetchTrendingShowsAsync()
                );


        }

        private async Task FetchCalendarAsync()
        {
            //var days = 7;
            //var startDate = StartOfWeek(DateTime.Today, DayOfWeek.Monday);
            //var endDate = startDate.AddDays(days);
            //ThisWeek = (from day in await traktService.GetCalendarAsync(startDate.ToUniversalTime().AddDays(-1), days + 1, extended: TraktExtendEnum.IMAGES)
            //            from item in day.Value
            //            let airDate = DateTime.Parse(item.Airs_At, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)
            //            where airDate >= startDate && airDate <= endDate
            //            group item.Episode by airDate.DayOfWeek into groupItem
            //            orderby groupItem.Key
            //            select new GroupedEpisodes<DayOfWeek>
            //            {
            //                Key = groupItem.Key,
            //                Episodes = groupItem.ToList()
            //            }).ToList();
        }

        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }
}

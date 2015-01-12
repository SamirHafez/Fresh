using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Core.Models;

namespace Fresh.Windows.ViewModels
{
    public class TVShowPageViewModel : ViewModel, ITVShowPageViewModel
    {
        public INavigationService navigationService { get; private set; }
        private readonly ITraktService traktService;

        public TVShow Show { get; private set; }

        public TVShowPageViewModel(INavigationService navigationService, ITraktService traktService)
        {
            this.navigationService = navigationService;
            this.traktService = traktService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var showId = (int)navigationParameter;

            Show = TVShow.FromTrakt(await traktService.GetShowAsync(showId, extended: TraktExtendEnum.FULL_IMAGES));

            Title = Show.Title;
            Poster = Show.Poster;
            Overview = Show.Overview;
            Rating = Show.Rating;

            Seasons = new ObservableCollection<TraktSeason>(from season in await traktService.GetSeasonsAsync(Show.Id, extended: TraktExtendEnum.IMAGES)
                                                            orderby season.Number descending
                                                            select season);

            Comments = new ObservableCollection<TraktComment>(await traktService.GetShowCommentsAsync(Show.Id));
        }

        public DelegateCommand<ItemClickEventArgs> EnterSeasonCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(arg =>
                    navigationService.Navigate(App.Experience.Season.ToString(), new { season = ((TraktSeason)arg.ClickedItem).Number, showId = Show.Id }));
            }
        }

        public DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(args =>
                    {
                        var episode = (Episode)args.ClickedItem;
                        navigationService.Navigate(App.Experience.Episode.ToString(),
                            new { showId = episode.ShowId, season = episode.SeasonNumber, episode = episode.Number });
                    });
            }
        }

        string title = default(string);
        public string Title { get { return title; } set { SetProperty(ref title, value); } }

        string poster = default(string);
        public string Poster { get { return poster; } set { SetProperty(ref poster, value); } }

        string overview = default(string);
        public string Overview { get { return overview; } set { SetProperty(ref overview, value); } }

        double rating = default(double);
        public double Rating { get { return rating; } set { SetProperty(ref rating, value); } }

        ObservableCollection<TraktSeason> seasons = default(ObservableCollection<TraktSeason>);
        public ObservableCollection<TraktSeason> Seasons { get { return seasons; } set { SetProperty(ref seasons, value); } }

        ObservableCollection<TraktComment> comments = default(ObservableCollection<TraktComment>);
        public ObservableCollection<TraktComment> Comments { get { return comments; } set { SetProperty(ref comments, value); } }
    }
}

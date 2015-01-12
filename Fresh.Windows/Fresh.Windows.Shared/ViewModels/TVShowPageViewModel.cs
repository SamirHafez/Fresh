using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Fresh.Windows.Shared.Interfaces;
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

        public TraktTVShow Show { get; private set; }

        public TVShowPageViewModel(INavigationService navigationService, ITraktService traktService)
        {
            this.navigationService = navigationService;
            this.traktService = traktService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var showId = (int)navigationParameter;

            Show = await traktService.GetShowAsync(showId, extended: TraktExtendEnum.FULL_IMAGES);

            Title = Show.Title;
            Poster = Show.Images.Poster.Full;
            Overview = Show.Overview;
            Rating = Show.Rating;

            Progress = await traktService.GetShowWatchedProgressAsync(Show.Ids.Trakt, extended: TraktExtendEnum.IMAGES);

            Seasons = new ObservableCollection<TraktSeason>(from season in await traktService.GetSeasonsAsync(Show.Ids.Trakt, extended: TraktExtendEnum.IMAGES)
                                                            orderby season.Number descending
                                                            select season);

            Comments = new ObservableCollection<TraktComment>(await traktService.GetShowCommentsAsync(Show.Ids.Trakt));

            Related = new ObservableCollection<TraktTVShow>(await traktService.GetRelatedShowsAsync(Show.Ids.Trakt, extended: TraktExtendEnum.IMAGES));
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

        public DelegateCommand<ItemClickEventArgs> EnterSeasonCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(arg =>
                    navigationService.Navigate(App.Experience.Season.ToString(), new { season = ((TraktSeason)arg.ClickedItem).Number, showId = Show.Ids.Trakt }));
            }
        }

        public DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(args =>
                    {
                        var episode = (TraktEpisode)args.ClickedItem;
                        navigationService.Navigate(App.Experience.Episode.ToString(),
                            new { showId = Show.Ids.Trakt, season = episode.Season, episode = episode.Number });
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

        TraktWatchedProgress progress = new TraktWatchedProgress();
        public TraktWatchedProgress Progress { get { return progress; } set { SetProperty(ref progress, value); } }

        ObservableCollection<TraktSeason> seasons = default(ObservableCollection<TraktSeason>);
        public ObservableCollection<TraktSeason> Seasons { get { return seasons; } set { SetProperty(ref seasons, value); } }

        ObservableCollection<TraktComment> comments = default(ObservableCollection<TraktComment>);
        public ObservableCollection<TraktComment> Comments { get { return comments; } set { SetProperty(ref comments, value); } }

        ObservableCollection<TraktTVShow> related = default(ObservableCollection<TraktTVShow>);
        public ObservableCollection<TraktTVShow> Related { get { return related; } set { SetProperty(ref related, value); } }
    }
}

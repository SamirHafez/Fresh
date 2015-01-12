using System.Collections.Generic;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Fresh.Windows.Shared.Models;
using System;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Fresh.Windows.Core.Models;
using System.Collections.ObjectModel;

namespace Fresh.Windows.ViewModels
{
    public class EpisodePageViewModel : ViewModel, IEpisodePageViewModel
    {
        public INavigationService navigationService { get; private set; }
        private readonly ICrawlerService crawlerService;
        private readonly ITraktService traktService;

        public Episode Episode { get; private set; }

        private List<string> excludedLinks;

        public EpisodePageViewModel(INavigationService navigationService, ICrawlerService crawlerService, ITraktService traktService)
        {
            this.navigationService = navigationService;
            this.crawlerService = crawlerService;
            this.traktService = traktService;

            this.excludedLinks = new List<string>();
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            dynamic parameters = navigationParameter;
            var seasonNumber = (int)parameters.season;
            var showId = (int)parameters.showId;
            var episodeNumber = (int)parameters.episode;

            Episode = Episode.FromTrakt(await traktService.GetEpisodeAsync(showId, seasonNumber, episodeNumber, extended: TraktExtendEnum.FULL_IMAGES), showId);
            Episode.TVShow = TVShow.FromTrakt(await traktService.GetShowAsync(showId, extended: TraktExtendEnum.MIN));

            Number = Episode.Number;
            Title = Episode.Title;
            Overview = Episode.Overview;
            Screen = Episode.Screen;
            Watched = Episode.Watched;
            AirDate = Episode.AirDate.GetValueOrDefault();

            Comments = new ObservableCollection<TraktComment>(await traktService.GetEpisodeCommentsAsync(showId, seasonNumber, episodeNumber));

            if (Episode.Link == null && Episode.AirDate < DateTime.UtcNow)
                try
                {
                    Episode.Link = await crawlerService.GetLink(Episode.TVShow.Title, Episode.SeasonNumber, Episode.Number);
                }
                catch
                {

                }

            Link = Episode.Link;
        }

        public DelegateCommand MediaFailedCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    excludedLinks.Add(Episode.Link);
                    try
                    {
                        Episode.Link = await crawlerService.GetLink(Episode.TVShow.Title, Episode.SeasonNumber, Episode.Number, excludedLinks.ToArray());
                        Link = Episode.Link;
                    }
                    catch
                    {

                    }
                });
            }
        }

        public DelegateCommand ToggleWatchedCommand
        {
            get
            {
                return new DelegateCommand(ToggleWatched);
            }
        }

        private async void ToggleWatched()
        {
            Episode.Watched = Watched;
            await traktService.WatchEpisodesAsync(new List<int> { Episode.Id });
        }

        int number = default(int);
        public int Number { get { return number; } set { SetProperty(ref number, value); } }

        string title = default(string);
        public string Title { get { return title; } set { SetProperty(ref title, value); } }

        string overview = default(string);
        public string Overview { get { return overview; } set { SetProperty(ref overview, value); } }

        string link = default(string);
        public string Link { get { return link; } set { SetProperty(ref link, value); } }

        string screen = default(string);
        public string Screen { get { return screen; } set { SetProperty(ref screen, value); } }

        bool watched = default(bool);
        public bool Watched { get { return watched; } set { SetProperty(ref watched, value); } }

        DateTime airDate = default(DateTime);
        public DateTime AirDate { get { return airDate; } set { SetProperty(ref airDate, value); } }

        ObservableCollection<TraktComment> comments = default(ObservableCollection<TraktComment>);
        public ObservableCollection<TraktComment> Comments { get { return comments; } set { SetProperty(ref comments, value); } }
    }
}

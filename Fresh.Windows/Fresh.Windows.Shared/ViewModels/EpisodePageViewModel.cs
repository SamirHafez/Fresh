using System.Collections.Generic;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Fresh.Windows.Shared.Models;
using System;
using Fresh.Windows.Shared.Configuration;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace Fresh.Windows.ViewModels
{
    public class EpisodePageViewModel : ViewModel, IEpisodePageViewModel
    {
        public INavigationService navigationService { get; private set; }
        private readonly IStorageService storageService;
        private readonly ICrawlerService crawlerService;
        private readonly ITraktService traktService;
        private readonly ISession session;

        public Episode Episode { get; set; }

        private List<string> excludedLinks;

        public EpisodePageViewModel(IStorageService storageService, INavigationService navigationService, ICrawlerService crawlerService, ITraktService traktService, ISession session)
        {
            this.navigationService = navigationService;
            this.storageService = storageService;
            this.crawlerService = crawlerService;
            this.traktService = traktService;
            this.session = session;

            this.excludedLinks = new List<string>();
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var episodeId = (int)navigationParameter;

            Episode = await storageService.GetEpisodeAsync(episodeId);

            Number = Episode.Number;
            Title = Episode.Title;
            Overview = Episode.Overview;
            Screen = Episode.Screen;
            Watched = Episode.Watched;
            AirDate = Episode.AirDate.GetValueOrDefault();

            if (Episode.Link == null && Episode.AirDate < DateTime.UtcNow)
            {
                try
                {
                    Episode.Link = await crawlerService.GetLink(Episode.TVShow.Title, Episode.SeasonNumber, Episode.Number);
                    await storageService.UpdateEpisodeAsync(Episode);
                }
                catch
                {

                }
            }

            Link = Episode.Link;
        }

        public DelegateCommand LinkFailedCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    excludedLinks.Add(Episode.Link);
                    try
                    {
                        Episode.Link = await crawlerService.GetLink(Episode.TVShow.Title, Episode.SeasonNumber, Episode.Number, excludedLinks.ToArray());
                        await storageService.UpdateEpisodeAsync(Episode);
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

            await traktService.WatchEpisodesAsync(session.User.Username,
                Episode.TVShow.Title,
                Episode.TVShow.Year,
                new List<object> { new { season = Episode.SeasonNumber, episode = Episode.Number, last_Played = DateTime.UtcNow } });
            await storageService.UpdateEpisodeAsync(Episode);
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
    }
}

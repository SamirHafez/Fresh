using System.Collections.Generic;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Fresh.Windows.Shared.Models;
using System;

namespace Fresh.Windows.ViewModels
{
    public class EpisodePageViewModel : ViewModel, IEpisodePageViewModel
    {
        private readonly IStorageService storageService;
        private readonly ICrawlerService crawlerService;

        private Episode episode;

        public EpisodePageViewModel(IStorageService storageService, ICrawlerService crawlerService)
        {
            this.storageService = storageService;
            this.crawlerService = crawlerService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var episodeId = (int)navigationParameter;

            episode = await storageService.GetEpisodeAsync(episodeId);

            Number = episode.Number;
            Title = episode.Title;
            Overview = episode.Overview;
            Screen = episode.Screen;
            Watched = episode.Watched;

            if (episode.Link == null && episode.AirDate < DateTime.UtcNow)
            {
                try
                {
                    episode.Link = await crawlerService.GetLink(episode.Season.TVShow.Title, episode.Season.Number, episode.Number);
                    await storageService.UpdateEpisodeAsync(episode);
                }
                catch
                {

                }
            }

            Link = episode.Link;
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
            episode.Watched = Watched;

            await storageService.UpdateEpisodeAsync(episode);
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
    }
}

using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Fresh.Windows.Shared.Configuration;
using System.Linq;
using System;

namespace Fresh.Windows.ViewModels
{
    public class SeasonPageViewModel : ViewModel, ISeasonPageViewModel
    {
        private readonly IStorageService storageService;
        public INavigationService navigationService { get; private set; }
        private readonly ITraktService traktService;
        private readonly ISession session;

        public SeasonPageViewModel(IStorageService storageService, INavigationService navigationService, ITraktService traktService, ISession session)
        {
            this.storageService = storageService;
            this.navigationService = navigationService;
            this.traktService = traktService;
            this.session = session;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var seasonId = (int)navigationParameter;

            var season = await storageService.GetSeasonAsync(seasonId);

            Number = season.Number;
            Poster = season.Poster;
            Episodes = new ObservableCollection<Episode>(season.Episodes);
        }

        public DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(args => EpisodeSelected((Episode)args.ClickedItem));
            }
        }

        private void EpisodeSelected(Episode episode)
        {
            navigationService.Navigate(App.Experience.Episode.ToString(), episode.Id);
        }

        public DelegateCommand SeasonWatchedCommand
        {
            get
            {
                return new DelegateCommand(SeasonWatched);
            }
        }

        private async void SeasonWatched()
        {
            var episodes = (from episode in Episodes
                            where episode.Watched == false
                            select episode).ToList();

            if (episodes.Count == 0)
                return;

            await traktService.WatchEpisodesAsync(session.User.Username,
                session.User.Credential,
                episodes[0].Season.TVShow.Title,
                episodes[0].Season.TVShow.Year,
                new List<dynamic>(from episode in episodes
                                  select new { season = episode.Season.Number, episode = episode.Number, last_Played = DateTime.UtcNow }));

            foreach (var episode in episodes)
            {
                episode.Watched = true;
                await storageService.UpdateEpisodeAsync(episode);
            }
        }

        ObservableCollection<Episode> episodes = default(ObservableCollection<Episode>);
        public ObservableCollection<Episode> Episodes { get { return episodes; } set { SetProperty(ref episodes, value); } }

        int number = default(int);
        public int Number { get { return number; } set { SetProperty(ref number, value); } }

        string poster = default(string);
        public string Poster { get { return poster; } set { SetProperty(ref poster, value); } }
    }
}

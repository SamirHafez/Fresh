using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;

namespace Fresh.Windows.ViewModels
{
    public class SeasonPageViewModel : ViewModel, ISeasonPageViewModel
    {
        private readonly IStorageService storageService;
        private readonly INavigationService navigationService;

        public SeasonPageViewModel(IStorageService storageService, INavigationService navigationService)
        {
            this.storageService = storageService;
            this.navigationService = navigationService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var seasonId = (int)navigationParameter;

            var season = await storageService.GetSeasonAsync(seasonId);

            Number = season.Number;
            Episodes = new ObservableCollection<Episode>(season.Episodes);
        }

        public DelegateCommand<Episode> EpisodeSelectedCommand
        {
            get
            {
                return new DelegateCommand<Episode>(EpisodeSelected);
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
    }
}

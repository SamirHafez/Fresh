using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;

namespace Fresh.Windows.ViewModels
{
    public class TVShowPageViewModel : ViewModel, ITVShowPageViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IStorageService storageService;

        public TVShowPageViewModel(INavigationService navigationService, IStorageService storageService)
        {
            this.navigationService = navigationService;
            this.storageService = storageService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var showId = navigationParameter as string;

            var dbShow = await storageService.GetShowAsync(showId);

            Update(dbShow);
        }

        public DelegateCommand<Season> EnterSeasonCommand
        {
            get
            {
                return new DelegateCommand<Season>(EnterSeason);
            }
        }

        private void EnterSeason(Season season)
        {
            navigationService.Navigate(App.Experience.Season.ToString(), season.Id);
        }

        private void Update(TVShow fullShow)
        {
            Title = fullShow.Title;
            Poster = fullShow.Poster;

            Seasons = new ObservableCollection<Season>(fullShow.Seasons);
        }

        string title = default(string);
        public string Title { get { return title; } set { SetProperty(ref title, value); } }

        string poster = default(string);
        public string Poster { get { return poster; } set { SetProperty(ref poster, value); } }

        ObservableCollection<Season> seasons = default(ObservableCollection<Season>);
        public ObservableCollection<Season> Seasons { get { return seasons; } set { SetProperty(ref seasons, value); } }
    }
}

using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Configuration;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace Fresh.Windows.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly IStorageService storageService;
        private readonly INavigationService navigationService;
        private readonly ISession configurationService;

        public MainPageViewModel(ITraktService traktService, IStorageService storageService, INavigationService navigationService, ISession configurationService)
        {
            this.traktService = traktService;
            this.storageService = storageService;
            this.navigationService = navigationService;
            this.configurationService = configurationService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var username = configurationService.User.Username;

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username not provided.");

            Library = new ObservableCollection<TVShow>((await storageService.GetLibraryAsync()).OrderBy(show => show.Title));

            if (Library.Count == 0)
            {
                var traktLibrary = (await traktService.GetLibraryAsync(username, extended: true)).
                    Select(TVShow.FromTrakt).
                    ToList();

                Library = new ObservableCollection<TVShow>(traktLibrary);

                await storageService.UpdateLibraryAsync(Library);
            }
        }

        ObservableCollection<TVShow> library = default(ObservableCollection<TVShow>);
        public ObservableCollection<TVShow> Library { get { return library; } set { SetProperty(ref library, value); } }

        public DelegateCommand<TVShow> EnterShowCommand
        {
            get
            {
                return new DelegateCommand<TVShow>(EnterShow);
            }
        }

        private void EnterShow(TVShow show)
        {
            navigationService.Navigate(App.Experience.TVShow.ToString(), show.Id);
        }
    }
}

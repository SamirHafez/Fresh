using Fresh.Windows.Core.Configuration;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Interfaces;
using Fresh.Windows.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Navigation;

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
            var username = configurationService.Username;

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username not provided.");

            Library = new ObservableCollection<TVShow>(
                await storageService.HasKey("library") ?
                    await storageService.Get<IList<TVShow>>("library") :
                    (await traktService.GetLibrary(username, extended: true)).Select(TVShow.FromTrakt).ToList()
            );

            if (!await storageService.HasKey("library"))
                await storageService.Save("library", Library);
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

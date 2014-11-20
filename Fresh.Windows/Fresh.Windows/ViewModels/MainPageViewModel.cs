using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Interfaces;
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
        private readonly IConfigurationService configurationService;

        public MainPageViewModel(ITraktService traktService, IStorageService storageService, INavigationService navigationService, IConfigurationService configurationService)
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

            Collection = new ObservableCollection<ITVShowPageViewModel>(
                await storageService.HasKey("collection") ?
                    await storageService.Get<IList<TVShowPageViewModel>>("collection") :
                    (await traktService.GetCollection(username)).Select(s => new TVShowPageViewModel(traktService) { Title = s.Title, Year = s.Year })
            );

            if (!await storageService.HasKey("collection"))
                await storageService.Save("collection", Collection);
        }

        ObservableCollection<ITVShowPageViewModel> collection = default(ObservableCollection<ITVShowPageViewModel>);
        public ObservableCollection<ITVShowPageViewModel> Collection { get { return collection; } set { SetProperty(ref collection, value); } }

        public DelegateCommand<TVShowPageViewModel> EnterShowCommand
        {
            get
            {
                return new DelegateCommand<TVShowPageViewModel>(EnterShow,
                    show => show != null);
            }
        }

        private void EnterShow(TVShowPageViewModel show)
        {
            navigationService.Navigate(App.Experience.TVShow.ToString(), show.Url.Substring(show.Url.LastIndexOf("/") + 1));
        }
    }
}

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
using System.Threading.Tasks;
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

            Collection = new ObservableCollection<TVShow>(
                await storageService.HasKey("collection") ?
                    await storageService.Get<IList<TVShow>>("collection") :
                    await GetCollectionWithWatched(username)
            );

            if (!await storageService.HasKey("collection"))
                await storageService.Save("collection", Collection);
        }

        private async Task<IList<TVShow>> GetCollectionWithWatched(string username)
        {
            var collection = await traktService.GetCollection(username, extended: true);



            return (await traktService.GetCollection(username, extended: true)).
                Select(TVShow.FromTrakt).
                ToList();
        }

        ObservableCollection<TVShow> collection = default(ObservableCollection<TVShow>);
        public ObservableCollection<TVShow> Collection { get { return collection; } set { SetProperty(ref collection, value); } }

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

using System.Collections.ObjectModel;
using Fresh.Windows.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using Fresh.Windows.Core.Services.Interfaces;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using System;
using System.Linq;
using Fresh.Windows.Core.Models;

namespace Fresh.Windows.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly IStorageService storageService;

        public MainPageViewModel(ITraktService traktService, IStorageService storageService)
        {
            this.traktService = traktService;
            this.storageService = storageService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var username = navigationParameter as string;

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username not provided.");

            var shows = await storageService.HasKey("collection") ?
                await storageService.Get<IList<TVShow>>("collection") :
                await traktService.GetCollection(username);

            Collection = new ObservableCollection<ITVShowViewModel>(shows.Select(s => new TVShowViewModel
            {
                Title = s.Title,
                Year = s.Year,
                Poster = s.Poster
            }));

            if (!await storageService.HasKey("collection"))
                await storageService.Save("collection", shows);
        }

        ObservableCollection<ITVShowViewModel> collection = default(ObservableCollection<ITVShowViewModel>);
        public ObservableCollection<ITVShowViewModel> Collection { get { return collection; } set { SetProperty(ref collection, value); } }
    }
}

using Fresh.Windows.Shared.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System;
using Fresh.Windows.Shared.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Fresh.Windows.Core.Services.Interfaces;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Fresh.Windows.Shared.Services.Interfaces;
using Windows.UI.Popups;
using Fresh.Windows.Shared.Configuration;
using Windows.UI.Xaml.Controls;

namespace Fresh.Windows.ViewModels
{
    public class SearchResultsPageViewModel : ViewModel, ISearchResultsPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly IStorageService storageService;
        private readonly ISession session;
        public INavigationService NavigationService { get; private set; }

        string searchQuery = default(string);
        public String SearchQuery { get { return searchQuery; } set { SetProperty(ref searchQuery, value); } }

        ObservableCollection<TVShow> tvShows = default(ObservableCollection<TVShow>);
        public ObservableCollection<TVShow> TVShows { get { return tvShows; } set { SetProperty(ref tvShows, value); } }

        public TVShow SelectedTVShow { get; set; }

        public SearchResultsPageViewModel(ITraktService traktService, INavigationService navigationService, IStorageService storageService, ISession session)
        {
            NavigationService = navigationService;
            this.storageService = storageService;
            this.traktService = traktService;
            this.session = session;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            SearchQuery = (string)navigationParameter;

            TVShows = new ObservableCollection<TVShow>(from traktTVShowResult in await traktService.SearchTVShowAsync(SearchQuery)
                                                       select TVShow.FromTrakt(traktTVShowResult.Show));

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public DelegateCommand<ItemClickEventArgs> GotoCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(arg => 
                    NavigationService.Navigate(App.Experience.TVShow.ToString(), ((TVShow)arg.ClickedItem).Id));
            }
        }
    }
}

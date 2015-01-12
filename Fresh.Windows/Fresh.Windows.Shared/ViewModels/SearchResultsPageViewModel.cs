using Fresh.Windows.Shared.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Fresh.Windows.Core.Services.Interfaces;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Fresh.Windows.Shared.Services.Interfaces;
using Windows.UI.Xaml.Controls;
using Fresh.Windows.Core.Models;

namespace Fresh.Windows.ViewModels
{
    public class SearchResultsPageViewModel : ViewModel, ISearchResultsPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly IStorageService storageService;
        public INavigationService NavigationService { get; private set; }

        string searchQuery = default(string);
        public String SearchQuery { get { return searchQuery; } set { SetProperty(ref searchQuery, value); } }

        ObservableCollection<TraktTVShow> tvShows = default(ObservableCollection<TraktTVShow>);
        public ObservableCollection<TraktTVShow> TVShows { get { return tvShows; } set { SetProperty(ref tvShows, value); } }

        public SearchResultsPageViewModel(ITraktService traktService, INavigationService navigationService, IStorageService storageService)
        {
            NavigationService = navigationService;
            this.storageService = storageService;
            this.traktService = traktService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            SearchQuery = (string)navigationParameter;

            TVShows = new ObservableCollection<TraktTVShow>(from traktTVShowResult in await traktService.SearchTVShowAsync(SearchQuery)
                                                            select traktTVShowResult.Show);

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public DelegateCommand<ItemClickEventArgs> GotoCommand
        {
            get
            {
                return new DelegateCommand<ItemClickEventArgs>(arg =>
                    NavigationService.Navigate(App.Experience.TVShow.ToString(), ((TraktTVShow)arg.ClickedItem).Ids.Trakt));
            }
        }
    }
}

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

            TVShows = new ObservableCollection<TVShow>(from traktTVShow in await traktService.SearchTVShowAsync(SearchQuery)
                                                       select TVShow.FromTrakt(traktTVShow));

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public DelegateCommand AddShowCommand
        {
            get
            {
                return new DelegateCommand(AddShow);
            }
        }

        private async void AddShow()
        {
            if (SelectedTVShow == null)
                return;

            if (await storageService.GetShowAsync(SelectedTVShow.Id) != null)
            {
                await new MessageDialog("the selected show already exists in your collection.", "error").ShowAsync();
                return;
            }

            var fullShow = TVShow.FromTrakt(await traktService.GetShowAsync(SelectedTVShow.Id, extended: true));

            await traktService.AddShowToLibraryAsync(session.User.Username,
                session.User.Credential, fullShow.Title, fullShow.Year);

            await storageService.UpdateShowAsync(fullShow);

            NavigationService.GoBack();
        }
    }
}

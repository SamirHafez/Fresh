using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace Fresh.Windows.Shared.DesignTime
{
    public class SearchResultsPageViewModel : ISearchResultsPageViewModel
    {
        public string SearchQuery { get; set; }

        public DelegateCommand<ItemClickEventArgs> GotoCommand { get { return null; } }
        public ObservableCollection<TVShow> TVShows { get; set; }

        public SearchResultsPageViewModel()
        {
            SearchQuery = "The Walking Dead";

            TVShows = new ObservableCollection<TVShow>
            {
                new TVShow { Title = "The Walking Dead", Poster = "http://slurm.trakt.us/images/posters/124.47.jpg" }
            };
        }
    }
}

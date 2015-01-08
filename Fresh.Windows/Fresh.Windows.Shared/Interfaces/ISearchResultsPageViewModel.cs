using Fresh.Windows.Shared.Models;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface ISearchResultsPageViewModel
    {
        string SearchQuery { get; set; }

        DelegateCommand<ItemClickEventArgs> GotoCommand { get; }
        ObservableCollection<TVShow> TVShows { get; set; }
    }
}

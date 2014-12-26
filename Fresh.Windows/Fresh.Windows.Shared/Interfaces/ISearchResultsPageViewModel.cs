using Fresh.Windows.Shared.Models;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface ISearchResultsPageViewModel
    {
        string SearchQuery { get; set; }

        TVShow SelectedTVShow { get; set; }
        ObservableCollection<TVShow> TVShows { get; set; }
    }
}

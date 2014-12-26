using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.DesignTime
{
    public class SearchResultsPageViewModel : ISearchResultsPageViewModel
    {
        public string SearchQuery { get; set; }

        public TVShow SelectedTVShow { get; set; }
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

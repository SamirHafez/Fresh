using Fresh.Windows.Core.Models;
using Fresh.Windows.Shared.Interfaces;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace Fresh.Windows.Shared.DesignTime
{
    public class SearchResultsPageViewModel : ISearchResultsPageViewModel
    {
        public string SearchQuery { get; set; }

        public DelegateCommand<ItemClickEventArgs> GotoCommand { get { return null; } }
        public ObservableCollection<TraktTVShow> TVShows { get; set; }

        public SearchResultsPageViewModel()
        {
            SearchQuery = "The Walking Dead";

            TVShows = new ObservableCollection<TraktTVShow>
            {
                new TraktTVShow { Title = "The Walking Dead", Images = new TraktImages { Poster = new TraktPoster { Thumb = "http://slurm.trakt.us/images/posters/124.47.jpg" } } },
                new TraktTVShow { Title = "The Walking Dead", Images = new TraktImages { Poster = new TraktPoster { Thumb = "http://slurm.trakt.us/images/posters/124.47.jpg" } } },
                new TraktTVShow { Title = "The Walking Dead", Images = new TraktImages { Poster = new TraktPoster { Thumb = "http://slurm.trakt.us/images/posters/124.47.jpg" } } },
                new TraktTVShow { Title = "The Walking Dead", Images = new TraktImages { Poster = new TraktPoster { Thumb = "http://slurm.trakt.us/images/posters/124.47.jpg" } } },
                new TraktTVShow { Title = "The Walking Dead", Images = new TraktImages { Poster = new TraktPoster { Thumb = "http://slurm.trakt.us/images/posters/124.47.jpg" } } }
            };
        }
    }
}

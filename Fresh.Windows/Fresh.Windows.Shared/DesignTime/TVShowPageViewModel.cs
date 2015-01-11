using Fresh.Windows.Core.Models;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace Fresh.Windows.Shared.DesignTime
{
    public class TVShowPageViewModel : ITVShowPageViewModel
    {
        public TVShowPageViewModel()
        {
            Title = "The Walking Dead";

            Poster = "http://slurm.trakt.us/images/posters/124.47.jpg";

            Rating = 8.5;

            Seasons = new ObservableCollection<TraktSeason>
            {
                new TraktSeason { Number = 1, Images = new TraktImages { Poster = new TraktPoster { Full = "http://slurm.trakt.us/images/seasons/124-5.47.jpg" } } },
                new TraktSeason { Number = 2, Images = new TraktImages { Poster = new TraktPoster { Full = "http://slurm.trakt.us/images/seasons/124-5.47.jpg" } } },
                new TraktSeason { Number = 3, Images = new TraktImages { Poster = new TraktPoster { Full = "http://slurm.trakt.us/images/seasons/124-5.47.jpg" } } },
            };

            Overview = "TVShow Overview Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras suscipit sem ante, sed volutpat sapien ultrices nec. Cras leo nunc, placerat ut est et, aliquam auctor nisi. Suspendisse elit sapien, finibus vitae venenatis et, gravida vitae justo. Aenean nec commodo ligula. Sed egestas sit amet dolor ut molestie. Etiam sed congue enim. Curabitur vitae finibus purus.";
        }

        public string Title { get; set; }
        public string Poster { get; set; }
        public ObservableCollection<TraktSeason> Seasons { get; set; }

        public string Overview { get; set; }

        public double Rating { get; set; }

        public DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand { get; set; }
        public DelegateCommand<ItemClickEventArgs> EnterSeasonCommand { get; set; }
    }
}

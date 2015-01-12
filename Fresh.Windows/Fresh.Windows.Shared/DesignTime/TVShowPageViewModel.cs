using Fresh.Windows.Core.Models;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Microsoft.Practices.Prism.Commands;
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

            Comments = new ObservableCollection<TraktComment>
            {
                new TraktComment { Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel augue tortor. Pellentesque faucibus turpis ut libero ullamcorper blandit.", User = new TraktUserInfo { Username = "Sam" } },
                new TraktComment { Comment = "Comment 2.", Spoiler = true, User = new TraktUserInfo { Username = "rhymecheat" } },
                new TraktComment { Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel augue tortor. Pellentesque faucibus turpis ut libero ullamcorper blandit.", User = new TraktUserInfo { Username = "123username" } },
                new TraktComment { Comment = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
                                Vivamus vel augue tortor. Pellentesque faucibus turpis ut libero ullamcorper blandit. 
                                Nullam iaculis, elit vel porttitor aliquet, odio ligula blandit nulla, sed gravida neque sem id nisl. 
                                Suspendisse potenti. Praesent scelerisque mauris ultricies rhoncus sollicitudin. 
                                Vestibulum mollis ultrices ligula, semper eleifend lorem egestas et. Sed lacus quam, pretium quis est sed, ultrices laoreet lectus. 
                                Morbi consequat dui sit amet tellus condimentum bibendum. Proin orci mi, commodo sed orci venenatis, congue interdum risus. 
                                Aliquam erat volutpat. Aenean tempor lorem ut felis ullamcorper, eu convallis ex facilisis. Donec lacinia velit eros, sit amet posuere ligula iaculis sed. 
                                Nulla sit amet arcu est. Nunc nec semper libero. Donec dictum arcu a elit posuere ultricies. 
                                Morbi ac nibh id lacus iaculis faucibus vel egestas nulla. Vivamus at urna vel justo volutpat molestie vel non velit. 
                                Quisque bibendum, sem vel varius commodo, turpis tortor tincidunt urna, ac vestibulum odio felis a lacus. Nullam vestibulum pellentesque velit, ac viverra nunc rutrum nec. 
                                Etiam nec luctus sem.", Review = true, Spoiler = true, User = new TraktUserInfo { Username = "US" } }
            };

            Related = new ObservableCollection<TVShow>
            {
                new TVShow { Title = "Community", Year = 2009, Poster = @"http://slurm.trakt.us/images/posters/5.19.jpg" },
                new TVShow { Title = "House of Cards (US)", Year = 2013, Poster = @"http://slurm.trakt.us/images/posters/19657.19.jpg" },
                new TVShow { Title = "House of Lies", Year = 2012, Poster = @"http://slurm.trakt.us/images/posters/11982.11.jpg" }
            };

            Overview = "TVShow Overview Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras suscipit sem ante, sed volutpat sapien ultrices nec. Cras leo nunc, placerat ut est et, aliquam auctor nisi. Suspendisse elit sapien, finibus vitae venenatis et, gravida vitae justo. Aenean nec commodo ligula. Sed egestas sit amet dolor ut molestie. Etiam sed congue enim. Curabitur vitae finibus purus.";
        }

        public string Title { get; set; }
        public string Poster { get; set; }
        public ObservableCollection<TraktSeason> Seasons { get; set; }
        public ObservableCollection<TraktComment> Comments { get; set; }
        public ObservableCollection<TVShow> Related { get; set; }

        public string Overview { get; set; }

        public double Rating { get; set; }

        public DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand { get; set; }
        public DelegateCommand<ItemClickEventArgs> EnterSeasonCommand { get; set; }
    }
}

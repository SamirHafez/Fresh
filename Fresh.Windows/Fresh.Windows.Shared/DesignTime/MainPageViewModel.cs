using Fresh.Windows.Interfaces;
using Fresh.Windows.Models;
using System.Collections.ObjectModel;

namespace Fresh.Windows.DesignTime
{
    public class MainPageViewModel : IMainPageViewModel
    {
        public MainPageViewModel()
        {
            Collection = new ObservableCollection<TVShow>
            {
                new TVShow { Title = "Community", Year = 2009, Images = new Images { Poster = @"http://slurm.trakt.us/images/posters/5.19.jpg" } },
                new TVShow { Title = "House of Cards (US)", Year = 2013, Images = new Images { Poster = @"http://slurm.trakt.us/images/posters/19657.19.jpg" } },
                new TVShow { Title = "House of Lies", Year = 2012, Images = new Images { Poster = @"http://slurm.trakt.us/images/posters/11982.11.jpg" } }
            };
        }

        public ObservableCollection<TVShow> Collection { get; set; }
    }
}

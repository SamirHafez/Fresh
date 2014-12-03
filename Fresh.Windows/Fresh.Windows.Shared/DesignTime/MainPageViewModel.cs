using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.DesignTime
{
    public class MainPageViewModel : IMainPageViewModel
    {
        public MainPageViewModel()
        {
            Library = new ObservableCollection<TVShow>
            {
                new TVShow { Title = "Community", Year = 2009, Poster = @"http://slurm.trakt.us/images/posters/5.19.jpg" },
                new TVShow { Title = "House of Cards (US)", Year = 2013, Poster = @"http://slurm.trakt.us/images/posters/19657.19.jpg" },
                new TVShow { Title = "House of Lies", Year = 2012, Poster = @"http://slurm.trakt.us/images/posters/11982.11.jpg" }
            };
        }

        public ObservableCollection<TVShow> Library { get; set; }
    }
}

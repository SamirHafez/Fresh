using System.Collections.ObjectModel;
using Fresh.Windows.Interfaces;

namespace Fresh.Windows.DesignTime
{
    public class MainPageViewModel : IMainPageViewModel
    {
        public MainPageViewModel()
        {
            Collection = new ObservableCollection<ITVShowViewModel>
            {
                new TVShowViewModel(),
                new TVShowViewModel { Title = "Community", Year = 2009, Images = new Images { Poster = @"http://slurm.trakt.us/images/posters/5.19.jpg" } },
                new TVShowViewModel { Title = "House of Cards (US)", Year = 2013, Images = new Images { Poster = @"http://slurm.trakt.us/images/posters/19657.19.jpg" } },
                new TVShowViewModel { Title = "House of Lies", Year = 2012, Images = new Images { Poster = @"http://slurm.trakt.us/images/posters/11982.11.jpg" } }
            };
        }

        public ObservableCollection<ITVShowViewModel> Collection { get; set; }
    }
}

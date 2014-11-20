using System.Collections.ObjectModel;
using Fresh.Windows.Interfaces;

namespace Fresh.Windows.DesignTime
{
    public class MainPageViewModel : IMainPageViewModel
    {
        public MainPageViewModel()
        {
            Collection = new ObservableCollection<ITVShowPageViewModel>
            {
                new TVShowPageViewModel(),
                new TVShowPageViewModel { Title = "Community", Year = 2009, Images = new Images { Poster = @"http://slurm.trakt.us/images/posters/5.19.jpg" } },
                new TVShowPageViewModel { Title = "House of Cards (US)", Year = 2013, Images = new Images { Poster = @"http://slurm.trakt.us/images/posters/19657.19.jpg" } },
                new TVShowPageViewModel { Title = "House of Lies", Year = 2012, Images = new Images { Poster = @"http://slurm.trakt.us/images/posters/11982.11.jpg" } }
            };
        }

        public ObservableCollection<ITVShowPageViewModel> Collection { get; set; }
    }
}

using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.DesignTime
{
    public class TVShowPageViewModel : ITVShowPageViewModel
    {
        public TVShowPageViewModel()
        {
            Title = "The Walking Dead";

            Poster = "http://slurm.trakt.us/images/posters/124.47.jpg";

            Seasons = new ObservableCollection<Season>
            {
                new Season
                {
                    Number = 5,
                    Poster = "http://slurm.trakt.us/images/seasons/124-5.47.jpg"
                }
            };
        }

        public string Title { get; set; }
        public string Poster { get; set; }
        public ObservableCollection<Season> Seasons { get; set; }
    }
}

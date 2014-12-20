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

            Rating = 85;

            Loved = 12000;
            Hated = 3200;

            Seasons = new ObservableCollection<Season>
            {
                new Season
                {
                    Number = 5,
                    Poster = "http://slurm.trakt.us/images/seasons/124-5.47.jpg"
                },
                new Season
                {
                    Number = 4,
                    Poster = "http://slurm.trakt.us/images/seasons/124-4.47.jpg"
                }
            };

            Overview = "TVShow Overview ipsum lorem.";
        }

        public string Title { get; set; }
        public string Poster { get; set; }
        public ObservableCollection<Season> Seasons { get; set; }

        public string Overview { get; set; }

        public int Rating { get; set; }

        public int Loved { get; set; }

        public int Hated { get; set; }
    }
}

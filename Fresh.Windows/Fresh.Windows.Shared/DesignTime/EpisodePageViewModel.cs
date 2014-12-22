using Fresh.Windows.Shared.Interfaces;
using System;

namespace Fresh.Windows.Shared.DesignTime
{
    public class EpisodePageViewModel : IEpisodePageViewModel
    {
        public EpisodePageViewModel()
        {
            Number = 1;
            Title = "Episode Title";
            Overview = "Episode overview.";
            Watched = true;
            Link = null;
            Screen = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg";

            AirDate = DateTime.UtcNow;
        }

        public string Link { get; set; }

        public int Number { get; set; }

        public string Overview { get; set; }

        public string Title { get; set; }

        public bool Watched { get; set; }

        public string Screen { get; set; }
        public DateTime AirDate { get; set; }
    }
}

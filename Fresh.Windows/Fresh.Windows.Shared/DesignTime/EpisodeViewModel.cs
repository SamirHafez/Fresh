using System;
using Fresh.Windows.Shared.Interfaces;

namespace Fresh.Windows.Shared.DesignTime
{
    public class EpisodeViewModel : IEpisodeViewModel
    {
        public string Link { get; set; }

        public int Number { get; set; }

        public int Season { get; set; }

        public string Screen { get; set; }

        public DateTime FirstAired { get; set; }

        public string Overview { get; set; }
    }
}

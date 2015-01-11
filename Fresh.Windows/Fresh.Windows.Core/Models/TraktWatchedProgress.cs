using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Models
{
    public class TraktWatchedProgressEpisode
    {
        public int number { get; set; }
        public bool completed { get; set; }
    }

    public class TraktWatchedProgressSeason
    {
        public int number { get; set; }
        public int aired { get; set; }
        public int completed { get; set; }
        public List<TraktWatchedProgressEpisode> episodes { get; set; }
    }

    public class TraktWatchedProgress
    {
        public int Aired { get; set; }
        public int Completed { get; set; }
        public List<TraktWatchedProgressSeason> Seasons { get; set; }
        public TraktEpisode Next_Episode { get; set; }
    }
}

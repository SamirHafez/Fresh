using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Models
{
    public class TraktActivityMovies
    {
        public string Watched_At { get; set; }
        public string Collected_At { get; set; }
        public string Rated_At { get; set; }
        public string Watchlisted_At { get; set; }
        public string Commented_At { get; set; }
        public string Paused_At { get; set; }
    }

    public class TraktActivityEpisodes
    {
        public string Watched_At { get; set; }
        public string Collected_At { get; set; }
        public string Rated_At { get; set; }
        public string Watchlisted_At { get; set; }
        public string Commented_At { get; set; }
        public string Paused_At { get; set; }
    }

    public class TraktActivityShows
    {
        public string Rated_At { get; set; }
        public string Watchlisted_At { get; set; }
        public string Commented_At { get; set; }
    }

    public class TraktActivitySeasons
    {
        public string Rated_At { get; set; }
        public string Watchlisted_At { get; set; }
        public string Commented_At { get; set; }
    }

    public class TraktActivityComments
    {
        public string Liked_At { get; set; }
    }

    public class TraktActivityLists
    {
        public string Liked_At { get; set; }
        public string Updated_At { get; set; }
    }

    public class TraktActivity
    {
        public string All { get; set; }
        public TraktActivityMovies Movies { get; set; }
        public TraktActivityEpisodes Episodes { get; set; }
        public TraktActivityShows Shows { get; set; }
        public TraktActivitySeasons Seasons { get; set; }
        public TraktActivityComments Comments { get; set; }
        public TraktActivityLists Lists { get; set; }
    }
}

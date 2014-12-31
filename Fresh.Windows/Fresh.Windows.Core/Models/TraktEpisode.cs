using System.Collections.Generic;

namespace Fresh.Windows.Core.Models
{
    public class TraktEpisode
    {
        public int Season { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public TraktIds Ids { get; set; }
        public int? Number_Abs { get; set; }
        public string Overview { get; set; }
        public string First_Aired { get; set; }
        public string Updated_At { get; set; }
        public double Rating { get; set; }
        public int Votes { get; set; }
        public int? Plays { get; set; }
        public List<string> Available_Translations { get; set; }
        public TraktImages Images { get; set; }
    }

    public class TraktWatchedShow
    {
        public int Plays { get; set; }
        public string Last_Watched_At { get; set; }
        public TraktTVShow Show { get; set; }
        public List<TraktWatchedSeason> Seasons { get; set; }
    }

    public class TraktWatchedSeason : TraktSeason
    {
        public List<TraktEpisode> Episodes { get; set; }
    }
}

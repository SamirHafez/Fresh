using Fresh.Windows.Core.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace Fresh.Windows.Shared.Models
{
    public class Season
    {
        [PrimaryKey]
        public int Number { get; set; }

        [PrimaryKey]
        public string ShowId { get; set; }

        public string Url { get; set; }
        public string Poster { get; set; }
        public IList<Episode> Episodes { get; set; }

        public static Season FromTrakt(TraktSeason trakt)
        {
            return new Season
            {
                Number = trakt.Season,
                Url = trakt.Url,
                Poster = trakt.Poster,
                Episodes = trakt.Episodes.Select(Episode.FromTrakt).ToList()
            };
        }
    }
}

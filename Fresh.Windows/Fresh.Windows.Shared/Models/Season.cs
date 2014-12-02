using Fresh.Windows.Core.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Fresh.Windows.Shared.Models
{
    public class Season
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int Number { get; set; }

        [ForeignKey(typeof(TVShow))]
        public string ShowId { get; set; }
        public string Url { get; set; }
        public string Poster { get; set; }

        [ManyToOne]
        public TVShow TVShow { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Episode> Episodes { get; set; }

        public static Season FromTrakt(TraktSeason trakt)
        {
            return new Season
            {
                Number = trakt.Season,
                Url = trakt.Url,
                Poster = trakt.Poster,
                Episodes = trakt.Episodes != null ? new List<Episode>(trakt.Episodes.Select(Episode.FromTrakt)) : null
            };
        }
    }
}

using Fresh.Windows.Core.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace Fresh.Windows.Shared.Models
{
    public class Season
    {
        [PrimaryKey]
        public int Id { get; set; }

        [ForeignKey(typeof(TVShow))]
        public int TVShowId { get; set; }

        public int Number { get; set; }

        public double Rating { get; set; }
        public string Overview { get; set; }
        public string Poster { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Episode> Episodes { get; set; }

        [ManyToOne]
        public TVShow TVShow { get; set; }

        public static Season FromTrakt(TraktSeason trakt)
        {
            return new Season
            {
                Id = trakt.Ids.Tvdb,
                Number = trakt.Number,
                Overview = trakt.Overview,
                Rating = trakt.Rating,
                Poster = trakt.Images.Poster.Full
            };
        }
    }
}

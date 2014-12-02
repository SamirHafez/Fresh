using Fresh.Windows.Core.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Globalization;

namespace Fresh.Windows.Shared.Models
{
    public class Episode
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Season))]
        public int SeasonId { get; set; }

        public string Title { get; set; }
        public int Number { get; set; }
        public string Overview { get; set; }
        public string Url { get; set; }
        public string Screen { get; set; }
        public int TvdbId { get; set; }
        public int? Plays { get; set; }
        public bool Watched { get; set; }
        public string Link { get; set; }
        public DateTime FirstAired { get; set; }

        [ManyToOne]
        public Season Season { get; set; }

        public static Episode FromTrakt(TraktEpisode trakt)
        {
            return new Episode
            {
                Title = trakt.Title,
                Number = trakt.Number,
                Url = trakt.Url,
                FirstAired = DateTime.Parse(trakt.First_aired_iso ?? "1900-01-01", CultureInfo.InvariantCulture),
                Overview = trakt.Overview,
                TvdbId = trakt.Tvdb_id,
                Screen = trakt.Screen,
                Plays = trakt.Plays,
                Watched = false
            };
        }
    }
}

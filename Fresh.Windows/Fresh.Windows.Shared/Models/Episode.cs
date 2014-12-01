using Fresh.Windows.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Fresh.Windows.Shared.Models
{
    public class Episode
    {
        public string Title { get; set; }

        [PrimaryKey]
        public int Season { get; set; }

        [PrimaryKey]
        public int Number { get; set; }

        [PrimaryKey]
        public string ShowId { get; set; }

        public int EpisodeNumber { get; set; }
        public string Overview { get; set; }
        public string Url { get; set; }
        public string Screen { get; set; }
        public int TvdbId { get; set; }
        public int? Plays { get; set; }
        public bool Watched { get; set; }
        public IList<string> Links { get; set; }
        public DateTime FirstAired { get; set; }

        public static Episode FromTrakt(TraktEpisode trakt)
        {
            return new Episode
            {
                Title = trakt.Title,
                Season = trakt.Season,
                Number = trakt.Number,
                EpisodeNumber = trakt.Episode,
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

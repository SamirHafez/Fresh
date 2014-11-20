using Fresh.Windows.Core.Models;
using System;

namespace Fresh.Windows.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Season { get; set; }
        public int Number { get; set; }
        public int EpisodeNumber { get; set; }
        public string Overview { get; set; }
        public string Url { get; set; }
        public string Screen { get; set; }
        public int TvdbId { get; set; }
        public DateTime FirstAired { get; set; }
        public Ratings Ratings { get; set; }

        public static Episode FromTrakt(TraktEpisode trakt)
        {
            return new Episode
            {
                Title = trakt.Title,
                Season = trakt.Season,
                Number = trakt.Number,
                EpisodeNumber = trakt.Episode,
                Url = trakt.Url,
                FirstAired = new DateTime(trakt.First_aired),
                Overview = trakt.Overview,
                TvdbId = trakt.Tvdb_id,
                Screen = trakt.Screen,
                Ratings = trakt.Ratings != null ? new Ratings
                {
                    Percentage = trakt.Ratings.Percentage,
                    Loved = trakt.Ratings.Loved,
                    Hated = trakt.Ratings.Hated,
                    Votes = trakt.Ratings.Votes
                } : null
            };
        }
    }
}

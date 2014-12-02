using Fresh.Windows.Core.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Fresh.Windows.Shared.Models
{
    public class TVShow
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Url { get; set; }
        public DateTime FirstAired { get; set; }
        public string Country { get; set; }
        public string Overview { get; set; }
        public TimeSpan Runtime { get; set; }
        public string Status { get; set; }
        public string Network { get; set; }
        public DayOfWeek AirDay { get; set; }
        public TimeSpan AirTime { get; set; }
        public string ImdbId { get; set; }
        public int TvdbId { get; set; }
        public DateTime LastUpdate { get; set; }

        public string Poster { get; set; }

        public int Percentage { get; set; }
        public int Votes { get; set; }
        public int Loved { get; set; }
        public int Hated { get; set; }

        public int Collection { get; set; }
        public int Watchers { get; set; }
        public int Plays { get; set; }

        public string Genres { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Season> Seasons { get; set; }

        public static TVShow FromTrakt(TraktTVShow trakt)
        {
            return new TVShow
            {
                Id = trakt.Url.Substring(trakt.Url.LastIndexOf("/") + 1),
                Title = trakt.Title,
                Year = trakt.Year,
                Url = trakt.Url,
                FirstAired = new DateTime(trakt.First_aired),
                Country = trakt.Country,
                Overview = trakt.Overview,
                Runtime = TimeSpan.FromMinutes(trakt.Runtime),
                Status = trakt.Status,
                Network = trakt.Network,
                AirDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), trakt.Air_day, ignoreCase: true),
                AirTime = DateTime.ParseExact(trakt.Air_time, "h:mmtt", CultureInfo.InvariantCulture).TimeOfDay,
                ImdbId = trakt.Imdb_id,
                TvdbId = trakt.Tvdb_id,
                LastUpdate = new DateTime(trakt.Last_updated),
                Poster = trakt.Images != null ? trakt.Images.Poster : null,
                Percentage = trakt.Ratings != null ? trakt.Ratings.Percentage : 0,
                Loved = trakt.Ratings != null ? trakt.Ratings.Loved : 0,
                Hated = trakt.Ratings != null ? trakt.Ratings.Hated : 0,
                Votes = trakt.Ratings != null ? trakt.Ratings.Votes : 0,
                Plays = trakt.Stats != null ? trakt.Stats.Plays : 0,
                Collection = trakt.Stats != null ? trakt.Stats.Collection : 0,
                Watchers = trakt.Stats != null ? trakt.Stats.Watchers : 0,
                Genres = trakt.Genres != null ? string.Join(";", trakt.Genres) : null,
                Seasons = trakt.Seasons != null ? new List<Season>(trakt.Seasons.Select(Season.FromTrakt)) : null
            };
        }

        internal void Update(TVShow traktTvShow)
        {
            Title = traktTvShow.Title;
            Year = traktTvShow.Year;
            Url = traktTvShow.Url;
            FirstAired = traktTvShow.FirstAired;
            Country = traktTvShow.Country;
            Overview = traktTvShow.Overview;
            Runtime = traktTvShow.Runtime;
            Status = traktTvShow.Status;
            Network = traktTvShow.Network;
            AirDay = traktTvShow.AirDay;
            AirTime = traktTvShow.AirTime;
            ImdbId = traktTvShow.ImdbId;
            TvdbId = traktTvShow.TvdbId;
            LastUpdate = traktTvShow.LastUpdate;

            Poster = traktTvShow.Poster;

            Percentage = traktTvShow.Percentage;
            Votes = traktTvShow.Votes;
            Loved = traktTvShow.Loved;
            Hated = traktTvShow.Hated;

            Collection = traktTvShow.Collection;
            Watchers = traktTvShow.Watchers;
            Plays = traktTvShow.Plays;

            Genres = traktTvShow.Genres;
        }
    }
}

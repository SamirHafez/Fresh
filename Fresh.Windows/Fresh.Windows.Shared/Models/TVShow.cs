using Fresh.Windows.Core.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fresh.Windows.Shared.Models
{
    public class TVShow
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Overview { get; set; }
        public string Network { get; set; }
        public string Poster { get; set; }
        public DayOfWeek? AirDay { get; set; }
        public int Rating { get; set; }
        public int Loved { get; set; }
        public int Hated { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Season> Seasons { get; set; }

        public static TVShow FromTrakt(TraktTVShow trakt)
        {
            return new TVShow
            {
                Id = trakt.Url.Substring(trakt.Url.LastIndexOf("/") + 1),
                Title = trakt.Title,
                Year = trakt.Year,
                Overview = trakt.Overview,
                Network = trakt.Network,
                Rating = trakt.Ratings != null ? trakt.Ratings.Percentage : 0,
                Loved = trakt.Ratings != null ? trakt.Ratings.Loved : 0,
                Hated = trakt.Ratings != null ? trakt.Ratings.Hated : 0,
                Poster = trakt.Images != null ? trakt.Images.Poster : null,
                AirDay = !string.IsNullOrWhiteSpace(trakt.Air_day) && trakt.Air_day != "Daily" ? (DayOfWeek)Enum.Parse(typeof(DayOfWeek), trakt.Air_day, ignoreCase: true) : (DayOfWeek?)null,
                Seasons = trakt.Seasons != null ? new List<Season>(trakt.Seasons.Select(Season.FromTrakt)) : null
            };
        }
    }
}

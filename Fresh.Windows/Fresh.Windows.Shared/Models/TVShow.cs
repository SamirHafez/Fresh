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
                Poster = trakt.Images != null ? trakt.Images.Poster : null,
                AirDay = trakt.Air_day != null ? (DayOfWeek)Enum.Parse(typeof(DayOfWeek), trakt.Air_day, ignoreCase: true) : (DayOfWeek?)null,
                Seasons = trakt.Seasons != null ? new List<Season>(trakt.Seasons.Select(Season.FromTrakt)) : null
            };
        }
    }
}

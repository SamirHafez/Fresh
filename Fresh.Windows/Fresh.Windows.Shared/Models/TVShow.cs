using Fresh.Windows.Core.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace Fresh.Windows.Shared.Models
{
    public class TVShow
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Overview { get; set; }
        public string Network { get; set; }
        public string Poster { get; set; }
        public DayOfWeek? AirDay { get; set; }
        public double Rating { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Season> Seasons { get; set; }

        public static TVShow FromTrakt(TraktTVShow trakt)
        {
            return new TVShow
            {
                Id = trakt.Ids.Tvdb,
                Title = trakt.Title,
                Year = trakt.Year,
                Overview = trakt.Overview,
                Network = trakt.Network,
                Rating = trakt.Rating,
                Poster = trakt.Images.Poster.Full,
                AirDay = !string.IsNullOrWhiteSpace(trakt.Airs.Day) && trakt.Airs.Day != "Daily" ? (DayOfWeek)Enum.Parse(typeof(DayOfWeek), trakt.Airs.Day, ignoreCase: true) : (DayOfWeek?)null,
            };
        }
    }
}

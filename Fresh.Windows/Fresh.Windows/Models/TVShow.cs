using Fresh.Windows.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Fresh.Windows.Models
{
    public class TVShow
    {
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
        public int TvrageId { get; set; }
        public DateTime LastUpdate { get; set; }
        public Images Images { get; set; }
        public IList<TopWatcher> Top_watchers { get; set; }
        public IList<Episode> Episodes { get; set; }
        public Ratings Ratings { get; set; }
        public Stats Stats { get; set; }
        public IList<Actor> Actors { get; set; }
        public IList<string> Genres { get; set; }

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
                TvrageId = trakt.Tvrage_id,
                LastUpdate = new DateTime(trakt.Last_updated),
                Images = new Images { Poster = trakt.Images.Poster, Banner = trakt.Images.Banner, Fanart = trakt.Images.Fanart },
                //Top_watchers = trakt.Top_watchers,
                Episodes = trakt.Seasons != null ?
                    trakt.Seasons.SelectMany(s => s.Episodes).Select(Episode.FromTrakt).ToList() : 
                    trakt.Top_episodes.Select(Episode.FromTrakt).ToList(),
                //Ratings = trakt.Ratings,
                //Stats = trakt.Stats,
                //Actors = trakt.People.Actors,
                Genres = trakt.Genres
            };
        }
    }

    public class Images
    {
        public string Poster { get; set; }
        public string Fanart { get; set; }
        public string Banner { get; set; }
    }

    public class TopWatcher
    {
        public int Plays { get; set; }
        public string Username { get; set; }
        public string Full_name { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string Location { get; set; }
        public string About { get; set; }
        public long Joined { get; set; }
        public string Avatar { get; set; }
        public string Url { get; set; }
    }

    public class Ratings
    {
        public int Percentage { get; set; }
        public int Votes { get; set; }
        public int Loved { get; set; }
        public int Hated { get; set; }
    }

    public class Stats
    {
        public int Watchers { get; set; }
        public int Plays { get; set; }
        public int Scrobbles { get; set; }
        public int Scrobbles_unique { get; set; }
        public int Checkins { get; set; }
        public int Checkins_unique { get; set; }
        public int Collection { get; set; }
        public int Collection_unique { get; set; }
    }

    public class Actor
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public string Image { get; set; }
    }
}

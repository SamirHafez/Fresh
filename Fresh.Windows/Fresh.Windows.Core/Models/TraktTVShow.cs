using System.Collections.Generic;

namespace Fresh.Windows.Core.Models
{
    public class TraktImages
    {
        public string Poster { get; set; }
        public string Fanart { get; set; }
        public string Banner { get; set; }
    }

    public class TraktTopWatcher
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

    public class TraktTopEpisode : TraktEpisode
    {
        public int Plays { get; set; }
    }

    public class TraktRatings
    {
        public int Percentage { get; set; }
        public int Votes { get; set; }
        public int Loved { get; set; }
        public int Hated { get; set; }
    }

    public class TraktStats
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

    public class TraktActorImage
    {
        public string Headshot { get; set; }
    }

    public class TraktActor
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public TraktActorImage Images { get; set; }
    }

    public class TraktPeople
    {
        public IList<TraktActor> Actors { get; set; }
    }

    public class TraktTVShow
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Url { get; set; }
        public int First_aired { get; set; }
        public string First_aired_iso { get; set; }
        public int First_aired_utc { get; set; }
        public string Country { get; set; }
        public string Overview { get; set; }
        public int Runtime { get; set; }
        public string Status { get; set; }
        public string Network { get; set; }
        public string Air_day { get; set; }
        public string Air_day_utc { get; set; }
        public string Air_time { get; set; }
        public string Air_time_utc { get; set; }
        public string Certification { get; set; }
        public string Imdb_id { get; set; }
        public int Tvdb_id { get; set; }
        public int Tvrage_id { get; set; }
        public int Last_updated { get; set; }
        public string Poster { get; set; }
        public TraktImages Images { get; set; }
        public IList<TraktTopWatcher> Top_watchers { get; set; }
        public IList<TraktTopEpisode> Top_episodes { get; set; }
        public TraktRatings Ratings { get; set; }
        public TraktStats Stats { get; set; }
        public TraktPeople People { get; set; }
        public IList<string> Genres { get; set; }
        public IList<TraktSeason> Seasons { get; set; }
    }
}

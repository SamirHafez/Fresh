using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Interfaces
{
    public interface ITVShowViewModel
    {
        string Title { get; set; }
        int Year { get; set; }
        string Url { get; set; }
        int First_aired { get; set; }
        string First_aired_iso { get; set; }
        int First_aired_utc { get; set; }
        string Country { get; set; }
        string Overview { get; set; }
        int Runtime { get; set; }
        string Status { get; set; }
        string Network { get; set; }
        string Air_day { get; set; }
        string Air_day_utc { get; set; }
        string Air_time { get; set; }
        string Air_time_utc { get; set; }
        string Certification { get; set; }
        string Imdb_id { get; set; }
        int Tvdb_id { get; set; }
        int Tvrage_id { get; set; }
        int Last_updated { get; set; }
        string Poster { get; set; }
        Images Images { get; set; }
        ObservableCollection<TopWatcher> Top_watchers { get; set; }
        ObservableCollection<TopEpisode> Top_episodes { get; set; }
        Ratings Ratings { get; set; }
        Stats Stats { get; set; }
        People People { get; set; }
        ObservableCollection<string> Genres { get; set; }
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

    public class TopEpisode
    {
        public int Plays { get; set; }
        public int Season { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public long First_aired { get; set; }
        public string First_aired_iso { get; set; }
        public long First_aired_utc { get; set; }
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

    public class ActorImage
    {
        public string Headshot { get; set; }
    }

    public class Actor
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public ActorImage Images { get; set; }
    }

    public class People
    {
        public IList<Actor> Actors { get; set; }
    }
}

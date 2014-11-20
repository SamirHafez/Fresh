using System;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Interfaces
{
    public interface ITVShowPageViewModel
    {
        string Title { get; set; }
        int Year { get; set; }
        string Url { get; set; }
        DateTime FirstAired { get; set; }
        string Country { get; set; }
        string Overview { get; set; }
        TimeSpan Runtime { get; set; }
        string Status { get; set; }
        string Network { get; set; }
        DayOfWeek AirDay { get; set; }
        TimeSpan AirTime { get; set; }
        string ImdbId { get; set; }
        int TvdbId { get; set; }
        int TvrageId { get; set; }
        DateTime LastUpdate { get; set; }
        Images Images { get; set; }
        Ratings Ratings { get; set; }
        Stats Stats { get; set; }

        ObservableCollection<string> Genres { get; set; }
        ObservableCollection<IEpisodePageViewModel> Episodes { get; set; }
        ObservableCollection<Actor> Actors { get; set; }
        ObservableCollection<TopWatcher> Top_watchers { get; set; }
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

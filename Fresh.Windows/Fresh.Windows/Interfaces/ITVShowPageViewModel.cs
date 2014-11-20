using Fresh.Windows.Models;
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
        ObservableCollection<Episode> TopEpisodes { get; set; }
        ObservableCollection<Actor> Actors { get; set; }
        ObservableCollection<TopWatcher> TopWatchers { get; set; }
    }
}

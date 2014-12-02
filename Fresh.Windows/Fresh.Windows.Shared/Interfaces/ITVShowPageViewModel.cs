using Fresh.Windows.Shared.Models;
using System;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.Interfaces
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
        DateTime LastUpdate { get; set; }
        string Poster { get; set; }

        ObservableCollection<string> Genres { get; set; }
        ObservableCollection<Season> Seasons { get; set; }
    }
}

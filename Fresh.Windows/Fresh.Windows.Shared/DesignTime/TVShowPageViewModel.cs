using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Fresh.Windows.Shared.DesignTime
{
    public class TVShowPageViewModel : ITVShowPageViewModel
    {
        public TVShowPageViewModel()
        {
            Title = "The Walking Dead";
            Year = 2010;
            Url = "http://trakt.tv/show/the-walking-dead";
            FirstAired = new DateTime(1288573200);
            Country = "United States";
            Overview = "Based on the comic book series of the same name; The Walking Dead tells the story of a small group of survivors living in the aftermath of a zombie apocalypse.";
            Runtime = TimeSpan.FromMinutes(45);
            Status = "Continuing";
            Network = "AMC";
            AirDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), "Sunday", ignoreCase: true);
            AirTime = DateTime.ParseExact("6:00pm", "h:mmtt", CultureInfo.InvariantCulture).TimeOfDay;
            LastUpdate = new DateTime(1416247798);

            Poster = "http://slurm.trakt.us/images/posters/124.47.jpg";

            Seasons = new ObservableCollection<Season>
            {
                new Season
                {
                    Number = 5,
                    Url = "http://trakt.tv/show/the-walking-dead/season/5",
                    Poster = "http://slurm.trakt.us/images/seasons/124-5.47.jpg",
                    Episodes = new ObservableCollection<Episode>
                    {
                        new Episode { Title = "No Sanctuary", Season = 5, Number = 1, Plays = 8476, Url = "http://trakt.tv/show/the-walking-dead/season/5/episode/1", FirstAired = new DateTime(1413172800), Screen = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg" },
                        new Episode { Title = "Four Walls and a Roof", Season = 5, Number = 3, Plays = 8450, Url = "http://trakt.tv/show/the-walking-dead/season/5/episode/3", FirstAired = new DateTime(1414382400), Screen = "http://slurm.trakt.us/images/episodes/124-5-3.47.jpg" },
                        new Episode { Title = "Strangers", Season = 5, Number = 2, Plays = 8324, Url = "http://trakt.tv/show/the-walking-dead/season/5/episode/2", FirstAired = new DateTime(1413770400), Screen = "http://slurm.trakt.us/images/episodes/124-5-2.47.jpg" }
                    }
                }
            };

            TopEpisodes = new ObservableCollection<Episode>(Seasons[0].Episodes);

            Genres = new ObservableCollection<string> { "Drama", "Horror", "Suspense" };
        }

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
        public DateTime LastUpdate { get; set; }
        public string Poster { get; set; }
        public ObservableCollection<Season> Seasons { get; set; }
        public ObservableCollection<Episode> TopEpisodes { get; set; }
        public ObservableCollection<string> Genres { get; set; }
    }
}

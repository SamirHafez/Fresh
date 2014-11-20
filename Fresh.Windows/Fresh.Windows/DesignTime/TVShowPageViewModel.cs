using Fresh.Windows.Interfaces;
using Fresh.Windows.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Fresh.Windows.DesignTime
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
            ImdbId = "tt1520211";
            TvdbId = 153021;
            TvrageId = 25056;
            LastUpdate = new DateTime(1416247798);

            Images = new Images
            {
                Poster = "http://slurm.trakt.us/images/posters/124.47.jpg",
                Fanart = "http://slurm.trakt.us/images/fanart/124.47.jpg",
                Banner = "http://slurm.trakt.us/images/banners/124.47.jpg"
            };

            //Episodes = new ObservableCollection<IEpisodeViewModel> {
            //    new EpisodeViewModel { Plays = 8676, Season = 5, Number = 1, Title = "No Sanctuary", Url = "http://trakt.tv/show/the-walking-dead/season/5/episode/1", First_aired = 1413172800, First_aired_iso = "2014-10-12T21:00:00-04:00", First_aired_utc = 1413162000 },
            //    new EpisodeViewModel { Plays = 8450, Season = 5, Number = 3, Title = "Four Walls and a Roof", Url = "http://trakt.tv/show/the-walking-dead/season/5/episode/3", First_aired = 1414382400, First_aired_iso = "2014-10-26T21:00:00-04:00", First_aired_utc = 1414371600 }
            //};
            TopEpisodes = new ObservableCollection<Episode>
            {
                new Episode { Title = "No Sanctuary", Season = 5, Number = 1 },
                new Episode { Title = "Four Walls and a Roof", Season = 5, Number = 3 }
            };

            Ratings = new Ratings
            {
                Percentage = 87,
                Votes = 23707,
                Loved = 22579,
                Hated = 1128
            };

            Stats = new Stats
            {
                Watchers = 35688,
                Plays = 335051,
                Scrobbles = 281024,
                Scrobbles_unique = 22065,
                Checkins = 54027,
                Checkins_unique = 14465,
                Collection = 105842,
                Collection_unique = 7847
            };

            Actors = new ObservableCollection<Actor>
            {
                new Actor { Name = "Andrew Lincoln", Character = "Rick Grimes", Image = "http://slurm.trakt.us/images/avatar-large.jpg" },
                new Actor { Name = "Sarah Wayne Callies", Character = "Lori Grimes", Image = "http://slurm.trakt.us/images/avatar-large.jpg" },
                new Actor { Name = "Steven Yeun", Character = "Glenn", Image = "http://slurm.trakt.us/images/avatar-large.jpg" }
            };

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
        public string ImdbId { get; set; }
        public int TvdbId { get; set; }
        public int TvrageId { get; set; }
        public DateTime LastUpdate { get; set; }
        public Images Images { get; set; }
        public ObservableCollection<TopWatcher> TopWatchers { get; set; }
        public ObservableCollection<Episode> TopEpisodes { get; set; }
        public Ratings Ratings { get; set; }
        public Stats Stats { get; set; }
        public ObservableCollection<Actor> Actors { get; set; }
        public ObservableCollection<string> Genres { get; set; }
    }
}

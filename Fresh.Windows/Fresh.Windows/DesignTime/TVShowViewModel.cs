using Fresh.Windows.Interfaces;
using System.Collections.ObjectModel;

namespace Fresh.Windows.DesignTime
{
    public class TVShowViewModel : ITVShowViewModel
    {
        public TVShowViewModel()
        {
            Title = "The Walking Dead";
            Year = 2010;
            Url = "http://trakt.tv/show/the-walking-dead";
            First_aired = 1288584000;
            First_aired_iso = "2010-10-31T21:00:00-04:00";
            First_aired_utc = 1288573200;
            Country = "United States";
            Overview = "Based on the comic book series of the same name; The Walking Dead tells the story of a small group of survivors living in the aftermath of a zombie apocalypse.";
            Runtime = 45;
            Status = "Continuing";
            Network = "AMC";
            Air_day = "Sunday";
            Air_day_utc = "Sunday";
            Air_time = "9:00pm";
            Air_time_utc = "6:00pm";
            Certification = "TV - MA";
            Imdb_id = "tt1520211";
            Tvdb_id = 153021;
            Tvrage_id = 25056;
            Last_updated = 1416247798;
            Poster = "http://slurm.trakt.us/images/posters/124.47.jpg";

            Images = new Images {
                Poster = "http://slurm.trakt.us/images/posters/124.47.jpg",
                Fanart = "http://slurm.trakt.us/images/fanart/124.47.jpg",
                Banner = "http://slurm.trakt.us/images/banners/124.47.jpg"
            };

            Top_episodes = new ObservableCollection<TopEpisode> {
                new TopEpisode { Plays = 8676, Season = 5, Number = 1, Title = "No Sanctuary", Url = "http://trakt.tv/show/the-walking-dead/season/5/episode/1", First_aired = 1413172800, First_aired_iso = "2014-10-12T21:00:00-04:00", First_aired_utc = 1413162000 },
                new TopEpisode { Plays = 8450, Season = 5, Number = 3, Title = "Four Walls and a Roof", Url = "http://trakt.tv/show/the-walking-dead/season/5/episode/3", First_aired = 1414382400, First_aired_iso = "2014-10-26T21:00:00-04:00", First_aired_utc = 1414371600 }
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

            People = new People
            {
                Actors = new ObservableCollection<Actor> {
                new Actor { Name = "Andrew Lincoln", Character = "Rick Grimes", Images = new ActorImage { Headshot = "http://slurm.trakt.us/images/avatar-large.jpg" } },
                new Actor { Name = "Sarah Wayne Callies", Character = "Lori Grimes", Images = new ActorImage { Headshot = "http://slurm.trakt.us/images/avatar-large.jpg" } },
                new Actor { Name = "Steven Yeun", Character = "Glenn", Images = new ActorImage { Headshot = "http://slurm.trakt.us/images/avatar-large.jpg" } }
                }
            };

            Genres = new ObservableCollection<string> { "Drama", "Horror", "Suspense" };
        }

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
        public Images Images { get; set; }
        public ObservableCollection<TopWatcher> Top_watchers { get; set; }
        public ObservableCollection<TopEpisode> Top_episodes { get; set; }
        public Ratings Ratings { get; set; }
        public Stats Stats { get; set; }
        public People People { get; set; }
        public ObservableCollection<string> Genres { get; set; }
    }
}

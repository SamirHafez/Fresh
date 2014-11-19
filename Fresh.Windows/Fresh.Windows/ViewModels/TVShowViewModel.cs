using Fresh.Windows.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Fresh.Windows.ViewModels
{
    public class TVShowViewModel : ViewModel, ITVShowViewModel
    {
        string title = default(string);
        public string Title { get { return title; } set { SetProperty(ref title, value); } }

        int year = default(int);
        public int Year { get { return year; } set { SetProperty(ref year, value); } }

        string poster = default(string);
        public string Poster { get { return poster; } set { SetProperty(ref poster, value); } }

        string url = default(string);
        public string Url { get { return url; } set { SetProperty(ref url, value); } }

        int first_aired = default(int);
        public int First_aired { get { return first_aired; } set { SetProperty(ref first_aired, value); } }

        string first_aired_iso = default(string);
        public string First_aired_iso { get { return first_aired_iso; } set { SetProperty(ref first_aired_iso, value); } }

        int first_aired_utc = default(int);
        public int First_aired_utc { get { return first_aired_utc; } set { SetProperty(ref first_aired_utc, value); } }

        string country = default(string);
        public string Country { get { return country; } set { SetProperty(ref country, value); } }

        string overview = default(string);
        public string Overview { get { return overview; } set { SetProperty(ref overview, value); } }

        int runtime = default(int);
        public int Runtime { get { return runtime; } set { SetProperty(ref runtime, value); } }

        string status = default(string);
        public string Status { get { return status; } set { SetProperty(ref status, value); } }

        string network = default(string);
        public string Network { get { return network; } set { SetProperty(ref network, value); } }

        string air_day = default(string);
        public string Air_day { get { return air_day; } set { SetProperty(ref air_day, value); } }

        string air_day_utc = default(string);
        public string Air_day_utc { get { return air_day_utc; } set { SetProperty(ref air_day_utc, value); } }

        string air_time = default(string);
        public string Air_time { get { return air_time; } set { SetProperty(ref air_time, value); } }

        string air_time_utc = default(string);
        public string Air_time_utc { get { return air_time_utc; } set { SetProperty(ref air_time_utc, value); } }

        string certification = default(string);
        public string Certification { get { return certification; } set { SetProperty(ref certification, value); } }

        string imdb_id = default(string);
        public string Imdb_id { get { return imdb_id; } set { SetProperty(ref imdb_id, value); } }

        int tvdb_id = default(int);
        public int Tvdb_id { get { return tvdb_id; } set { SetProperty(ref tvdb_id, value); } }

        int tvrage_id = default(int);
        public int Tvrage_id { get { return tvrage_id; } set { SetProperty(ref tvrage_id, value); } }

        int last_updated = default(int);
        public int Last_updated { get { return last_updated; } set { SetProperty(ref last_updated, value); } }

        Images images = default(Images);
        public Images Images { get { return images; } set { SetProperty(ref images, value); } }

        Ratings ratings = default(Ratings);
        public Ratings Ratings { get { return ratings; } set { SetProperty(ref ratings, value); } }

        Stats stats = default(Stats);
        public Stats Stats { get { return stats; } set { SetProperty(ref stats, value); } }

        People people = default(People);
        public People People { get { return people; } set { SetProperty(ref people, value); } }

        ObservableCollection<TopWatcher> top_watchers = default(ObservableCollection<TopWatcher>);
        public ObservableCollection<TopWatcher> Top_watchers { get { return top_watchers; } set { SetProperty(ref top_watchers, value); } }

        ObservableCollection<TopEpisode> top_episodes = default(ObservableCollection<TopEpisode>);
        public ObservableCollection<TopEpisode> Top_episodes { get { return top_episodes; } set { SetProperty(ref top_episodes, value); } }

        ObservableCollection<string> genres = default(ObservableCollection<string>);
        public ObservableCollection<string> Genres { get { return genres; } set { SetProperty(ref genres, value); } } 
    }
}

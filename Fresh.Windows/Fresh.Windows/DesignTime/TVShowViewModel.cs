using Fresh.Windows.Interfaces;

namespace Fresh.Windows.DesignTime
{
    public class TVShowViewModel : ITVShowViewModel
    {
        public TVShowViewModel()
        {
            Title = "Brooklyn Nine-Nine";
            Year = 2013;
            Poster = @"http://slurm.trakt.us/images/posters/23363.10.jpg";
        }

        public string Title { get; set; } 
        public int Year { get; set; }
        public string Poster { get; set; }
    }
}

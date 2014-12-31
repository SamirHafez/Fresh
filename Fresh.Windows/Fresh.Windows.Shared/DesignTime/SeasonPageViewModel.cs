using System.Collections.ObjectModel;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;

namespace Fresh.Windows.Shared.DesignTime
{
    public class SeasonPageViewModel : ISeasonPageViewModel
    {
        public int Number { get; set; }
        public string Poster { get; set; }
        public ObservableCollection<Episode> Episodes { get; set; }

        public SeasonPageViewModel()
        {
            Number = 2;

            Poster = "http://slurm.trakt.us/images/seasons/124-5.47.jpg";

            Episodes = new ObservableCollection<Episode>
                    {
                        new Episode
                        {
                            Number = 1,
                            Title = "Episode 1",
                            Screen = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg",
                        },
                        new Episode
                        {
                            Number = 3,
                            Title = "Episode 2",
                            Screen = "http://slurm.trakt.us/images/episodes/124-5-3.47.jpg",
                            Watched = true, 
                        },
                        new Episode
                        {
                            Number = 2,
                            Title = "Episode 3",
                            Screen = "http://slurm.trakt.us/images/episodes/124-5-2.47.jpg",
                        }
                    };
        }
    }
}

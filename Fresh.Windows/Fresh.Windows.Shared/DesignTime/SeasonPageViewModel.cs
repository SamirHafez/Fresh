using System.Collections.ObjectModel;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;

namespace Fresh.Windows.Shared.DesignTime
{
    public class SeasonPageViewModel : ISeasonPageViewModel
    {
        public int Number { get; set; }
        public ObservableCollection<Episode> Episodes { get; set; }

        public SeasonPageViewModel()
        {
            Number = 2;

            var season = new Season { TVShow = new TVShow { Title = "The Walking Dead" } };

            Episodes = new ObservableCollection<Episode>
                    {
                        new Episode
                        {
                            Number = 1,
                            Title = "Episode 1",
                            Screen = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg",
                            Season = season
                        },
                        new Episode
                        {
                            Number = 3,
                            Title = "Episode 2",
                            Screen = "http://slurm.trakt.us/images/episodes/124-5-3.47.jpg",
                            Watched = true,
                            Season = season
                        },
                        new Episode
                        {
                            Number = 2,
                            Title = "Episode 3",
                            Screen = "http://slurm.trakt.us/images/episodes/124-5-2.47.jpg",
                            Season = season
                        }
                    };
        }
    }
}

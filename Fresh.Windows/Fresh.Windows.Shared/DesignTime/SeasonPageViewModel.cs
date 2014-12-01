using System.Collections.ObjectModel;
using Fresh.Windows.Shared.Interfaces;

namespace Fresh.Windows.Shared.DesignTime
{
    public class SeasonPageViewModel : ISeasonPageViewModel
    {
        public int Number { get; set; }
        public ObservableCollection<IEpisodeViewModel> Episodes { get; set; }

        public SeasonPageViewModel()
        {
            Number = 2;

            Episodes = new ObservableCollection<IEpisodeViewModel>
                    {
                        new EpisodeViewModel { Season = 5, Number = 1, Screen = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg" },
                        new EpisodeViewModel { Season = 5, Number = 3, Screen = "http://slurm.trakt.us/images/episodes/124-5-3.47.jpg" },
                        new EpisodeViewModel { Season = 5, Number = 2, Screen = "http://slurm.trakt.us/images/episodes/124-5-2.47.jpg" }
                    };
        }
    }
}

using Fresh.Windows.Core.Models;
using Fresh.Windows.Shared.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.DesignTime
{
    public class EpisodePageViewModel : IEpisodePageViewModel
    {
        public EpisodePageViewModel()
        {
            Number = 1;
            Title = "Episode Title";
            Overview = "Episode overview.";
            Watched = true;
            Link = null;
            Screen = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg";

            AirDate = DateTime.UtcNow;

            Comments = new ObservableCollection<TraktComment>
            {
                new TraktComment { Comment = "Comment 1.", User = new TraktUserInfo { Username = "Sam" } },
                new TraktComment { Comment = "Comment 2.", User = new TraktUserInfo { Username = "Sam" } },
                new TraktComment { Comment = "Comment 3.", User = new TraktUserInfo { Username = "Sam" } },
                new TraktComment { Comment = "Comment 4.", User = new TraktUserInfo { Username = "Sam" } }
            };
        }

        public string Link { get; set; }

        public int Number { get; set; }

        public string Overview { get; set; }

        public string Title { get; set; }

        public bool Watched { get; set; }

        public string Screen { get; set; }
        public DateTime AirDate { get; set; }
        public ObservableCollection<TraktComment> Comments { get; set; }
    }
}

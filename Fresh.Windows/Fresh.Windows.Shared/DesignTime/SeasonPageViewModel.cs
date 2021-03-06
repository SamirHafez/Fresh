﻿using System.Collections.ObjectModel;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Core.Models;

namespace Fresh.Windows.Shared.DesignTime
{
    public class SeasonPageViewModel : ISeasonPageViewModel
    {
        public int Number { get; set; }
        public string Poster { get; set; }
        public string Overview { get; set; }
        public ObservableCollection<TraktEpisode> Episodes { get; set; }

        public SeasonPageViewModel()
        {
            Number = 2;

            Overview = "Season 2 Overview.";

            Poster = "http://slurm.trakt.us/images/seasons/124-5.47.jpg";

            Episodes = new ObservableCollection<TraktEpisode>
                    {
                        new TraktEpisode
                        {
                            Number = 1,
                            Title = "Episode 1",
                            Images = new TraktImages { Screenshot = new TraktScreenshot { Thumb = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg" } }
                        },
                        new TraktEpisode
                        {
                            Number = 3,
                            Title = "Episode 2",
                            Images = new TraktImages { Screenshot = new TraktScreenshot { Thumb = "http://slurm.trakt.us/images/episodes/124-5-2.47.jpg" } }
                        },
                        new TraktEpisode
                        {
                            Number = 2,
                            Title = "Episode 3",
                            Images = new TraktImages { Screenshot = new TraktScreenshot { Thumb = "http://slurm.trakt.us/images/episodes/124-5-3.47.jpg" } }
                        }
                    };
        }
    }
}

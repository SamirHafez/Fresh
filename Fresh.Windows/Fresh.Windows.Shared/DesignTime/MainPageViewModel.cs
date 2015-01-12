using Fresh.Windows.Core.Models;
using Fresh.Windows.Shared.Interfaces;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace Fresh.Windows.Shared.DesignTime
{
    public class MainPageViewModel : IMainPageViewModel
    {
        public MainPageViewModel()
        {
            Recommended = new ObservableCollection<TraktTVShow>
            {
                new TraktTVShow { Title = "Community", Year = 2009, Images = new TraktImages { Poster = new TraktPoster { Thumb = @"http://slurm.trakt.us/images/posters/5.19.jpg" } } },
                new TraktTVShow { Title = "House of Cards (US)", Year = 2013, Images = new TraktImages { Poster = new TraktPoster { Thumb = @"http://slurm.trakt.us/images/posters/19657.19.jpg" } } },
                new TraktTVShow { Title = "House of Lies", Year = 2012, Images = new TraktImages { Poster = new TraktPoster { Thumb = @"http://slurm.trakt.us/images/posters/11982.11.jpg" } } }
            };

            Trending = new ObservableCollection<TraktTVShow>
            {
                new TraktTVShow { Title = "Community", Year = 2009, Images = new TraktImages { Poster = new TraktPoster { Thumb = @"http://slurm.trakt.us/images/posters/5.19.jpg" } } },
                new TraktTVShow { Title = "House of Cards (US)", Year = 2013, Images = new TraktImages { Poster = new TraktPoster { Thumb = @"http://slurm.trakt.us/images/posters/19657.19.jpg" } } },
                new TraktTVShow { Title = "House of Lies", Year = 2012, Images = new TraktImages { Poster = new TraktPoster { Thumb = @"http://slurm.trakt.us/images/posters/11982.11.jpg" } } }
            };

            Popular = new ObservableCollection<TraktTVShow>
            {
                new TraktTVShow { Title = "Community", Year = 2009, Images = new TraktImages { Poster = new TraktPoster { Thumb = @"http://slurm.trakt.us/images/posters/5.19.jpg" } } },
                new TraktTVShow { Title = "House of Cards (US)", Year = 2013, Images = new TraktImages { Poster = new TraktPoster { Thumb = @"http://slurm.trakt.us/images/posters/19657.19.jpg" } } },
                new TraktTVShow { Title = "House of Lies", Year = 2012, Images = new TraktImages { Poster = new TraktPoster { Thumb = @"http://slurm.trakt.us/images/posters/11982.11.jpg" } } }
            };

            NextEpisodes = new ObservableCollection<TraktEpisode>
            {
                        new TraktEpisode
                        {
                            Title = "Episode Title 1",
                            Images = new TraktImages { Screenshot = new TraktScreenshot { Thumb = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg" } },
                            First_Aired = DateTime.Now.ToString()
                        },
                        new TraktEpisode
                        {
                            Title = "Episode Title 2",
                            Images = new TraktImages { Screenshot = new TraktScreenshot { Thumb = "http://slurm.trakt.us/images/episodes/124-5-2.47.jpg" } }, 
                            First_Aired = DateTime.Now.ToString()
                        },
                        new TraktEpisode
                        {
                            Title = "Episode Title 3",
                            Images = new TraktImages { Screenshot = new TraktScreenshot { Thumb = "http://slurm.trakt.us/images/episodes/124-5-3.47.jpg" } },
                            First_Aired = DateTime.Now.ToString()
                        }
            };

            ThisWeek = new List<GroupedEpisodes<DayOfWeek>>
            {
                new GroupedEpisodes<DayOfWeek>
                {
                    Key = DayOfWeek.Monday,
                    Episodes = new List<TraktEpisode>
                    {
                        new TraktEpisode
                        {
                            Title = "Episode Title 1",
                            Images = new TraktImages { Screenshot = new TraktScreenshot { Thumb = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg" } },
                            First_Aired = DateTime.Now.ToString()
                        }
                    }
                },
                new GroupedEpisodes<DayOfWeek>
                {
                    Key = DayOfWeek.Thursday,
                    Episodes = new List<TraktEpisode>
                    {
                        new TraktEpisode
                        {
                            Title = "Episode Title 2",
                            Images = new TraktImages { Screenshot = new TraktScreenshot { Thumb = "http://slurm.trakt.us/images/episodes/124-5-2.47.jpg" } },
                            First_Aired = DateTime.Now.ToString()
                        },
                        new TraktEpisode
                        {
                            Title = "Episode Title 3",
                            Images = new TraktImages { Screenshot = new TraktScreenshot { Thumb = "http://slurm.trakt.us/images/episodes/124-5-3.47.jpg" } },
                            First_Aired = DateTime.Now.ToString()
                        }
                    }
                }
            };
        }

        public ObservableCollection<TraktTVShow> Recommended { get; set; }
        public ObservableCollection<TraktTVShow> Popular { get; set; }
        public ObservableCollection<TraktTVShow> Trending { get; set; }
        public ObservableCollection<TraktEpisode> NextEpisodes { get; set; }
        public IList<GroupedEpisodes<DayOfWeek>> ThisWeek { get; set; }
        public bool Loading { get; set; }


        public DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand
        {
            get { throw new NotImplementedException(); }
        }

        public DelegateCommand<ItemClickEventArgs> EnterShowCommand
        {
            get { throw new NotImplementedException(); }
        }
    }
}

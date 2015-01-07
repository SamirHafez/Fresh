﻿using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
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
            Library = new ObservableCollection<TVShow>
            {
                new TVShow { Title = "Community", Year = 2009, Poster = @"http://slurm.trakt.us/images/posters/5.19.jpg" },
                new TVShow { Title = "House of Cards (US)", Year = 2013, Poster = @"http://slurm.trakt.us/images/posters/19657.19.jpg" },
                new TVShow { Title = "House of Lies", Year = 2012, Poster = @"http://slurm.trakt.us/images/posters/11982.11.jpg" }
            };

            UnwatchedEpisodesByShow = new ObservableCollection<GroupedEpisodes<TVShow>>
            {
                new GroupedEpisodes<TVShow>
                {
                    Key = new TVShow { Title = "Community", Year = 2009, Poster = @"http://slurm.trakt.us/images/posters/5.19.jpg" },
                    Episodes = new List<Episode>
                    {
                        new Episode
                        {
                            Title = "Episode Title 1", Screen = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg",
                            AirDate = DateTime.Now,
                        }
                    }
                },
                new GroupedEpisodes<TVShow>
                {
                    Key = new TVShow { Title = "House of Lies", Year = 2012, Poster = @"http://slurm.trakt.us/images/posters/11982.11.jpg" },
                    Episodes = new List<Episode>
                    {
                        new Episode
                        {
                            Title = "Episode Title 1", Screen = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg",
                            AirDate = DateTime.Now,
                        },
                        new Episode
                        {
                            Title = "Episode Title 2", Screen = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg",
                            AirDate = DateTime.Now,
                        }
                    }
                },
                new GroupedEpisodes<TVShow>
                {
                    Key = new TVShow { Title = "House of Cards (US)", Year = 2013, Poster = @"http://slurm.trakt.us/images/posters/19657.19.jpg" },
                    Episodes = new List<Episode>
                    {
                        new Episode
                        {
                            Title = "Episode Title 2", Screen = "http://slurm.trakt.us/images/episodes/124-5-2.47.jpg", Watched = true,
                            AirDate = DateTime.Now,
                        },
                        new Episode
                        {
                            Title = "Episode Title 3", Screen = "http://slurm.trakt.us/images/episodes/124-5-3.47.jpg",
                            AirDate = DateTime.Now,
                        }
                    }
                }
            };

            ThisWeek = new List<GroupedEpisodes<DayOfWeek>>
            {
                new GroupedEpisodes<DayOfWeek>
                {
                    Key = DayOfWeek.Monday,
                    Episodes = new List<Episode>
                    {
                        new Episode
                        {
                            Title = "Episode Title 1", Screen = "http://slurm.trakt.us/images/episodes/124-5-1.47.jpg",
                            AirDate = DateTime.Now,
                        }
                    }
                },
                new GroupedEpisodes<DayOfWeek>
                {
                    Key = DayOfWeek.Thursday,
                    Episodes = new List<Episode>
                    {
                        new Episode
                        {
                            Title = "Episode Title 2", Screen = "http://slurm.trakt.us/images/episodes/124-5-2.47.jpg", Watched = true,
                            AirDate = DateTime.Now,
                        },
                        new Episode
                        {
                            Title = "Episode Title 3", Screen = "http://slurm.trakt.us/images/episodes/124-5-3.47.jpg",
                            AirDate = DateTime.Now,
                        }
                    }
                }
            };
        }

        public ObservableCollection<TVShow> Library { get; set; }
        public ObservableCollection<GroupedEpisodes<TVShow>> UnwatchedEpisodesByShow { get; set; }
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

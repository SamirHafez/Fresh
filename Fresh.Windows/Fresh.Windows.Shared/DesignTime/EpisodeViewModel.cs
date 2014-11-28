﻿using System;
using System.Collections.ObjectModel;
using Fresh.Windows.Interfaces;

namespace Fresh.Windows.DesignTime
{
    public class EpisodeViewModel : IEpisodeViewModel
    {
        public ObservableCollection<string> Links { get; set; }

        public int Number { get; set; }

        public int Season { get; set; }

        public string Screen { get; set; }

        public DateTime FirstAired { get; set; }

        public string Overview { get; set; }
    }
}
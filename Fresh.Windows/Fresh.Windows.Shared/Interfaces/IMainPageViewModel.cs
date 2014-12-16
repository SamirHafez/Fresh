﻿using Fresh.Windows.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface IMainPageViewModel
    {
        ObservableCollection<TVShow> Library { get; set; }

        IList<GroupedEpisodes<DayOfWeek>> ThisWeek { get; set; }

        bool Loading { get; set; }
    }
}

using Fresh.Windows.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface IMainPageViewModel
    {
        ObservableCollection<TVShow> Library { get; set; }

        IEnumerable<GroupedEpisodes<DayOfWeek>> ThisWeek { get; set; }
    }
}

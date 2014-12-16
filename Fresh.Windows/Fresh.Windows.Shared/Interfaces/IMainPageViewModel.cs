using Fresh.Windows.Shared.Models;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface IMainPageViewModel
    {
        ObservableCollection<TVShow> Library { get; set; }

        IList<GroupedEpisodes<DayOfWeek>> ThisWeek { get; set; }

        bool Loading { get; set; }

        DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand { get; }
        DelegateCommand<ItemClickEventArgs> EnterShowCommand { get; }
    }
}

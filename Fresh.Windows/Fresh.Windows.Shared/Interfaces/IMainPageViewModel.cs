using Fresh.Windows.Core.Models;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface IMainPageViewModel
    {
        ObservableCollection<TraktTVShow> Recommended { get; set; }
        ObservableCollection<TraktTVShow> Popular { get; set; }
        ObservableCollection<TraktTVShow> Trending { get; set; }
        ObservableCollection<TraktEpisode> NextEpisodes { get; set; }

        IList<GroupedEpisodes<DayOfWeek>> ThisWeek { get; set; }

        bool Loading { get; set; }

        DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand { get; }
        DelegateCommand<ItemClickEventArgs> EnterShowCommand { get; }
    }

    public class GroupedEpisodes<T>
    {
        public T Key { get; set; }
        public IList<TraktEpisode> Episodes { get; set; }
    }
}

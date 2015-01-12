using Fresh.Windows.Core.Models;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface ITVShowPageViewModel
    {
        string Title { get; set; }
        string Poster { get; set; }
        string Overview { get; set; }
        double Rating { get; set; }

        ObservableCollection<TraktSeason> Seasons { get; set; }
        ObservableCollection<TraktComment> Comments { get; set; }

        DelegateCommand<ItemClickEventArgs> EnterSeasonCommand { get; }
        DelegateCommand<ItemClickEventArgs> EpisodeSelectedCommand { get; }
    }
}

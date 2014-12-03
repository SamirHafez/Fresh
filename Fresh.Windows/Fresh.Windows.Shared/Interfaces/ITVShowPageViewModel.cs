using Fresh.Windows.Shared.Models;
using System;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface ITVShowPageViewModel
    {
        string Title { get; set; }
        string Poster { get; set; }

        ObservableCollection<Season> Seasons { get; set; }
    }
}

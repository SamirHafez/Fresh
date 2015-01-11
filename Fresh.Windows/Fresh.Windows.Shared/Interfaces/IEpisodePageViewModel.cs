using Fresh.Windows.Core.Models;
using System;
using System.Collections.ObjectModel;
namespace Fresh.Windows.Shared.Interfaces
{
    public interface IEpisodePageViewModel
    {
        int Number { get; set; }

        string Title { get; set; }

        string Overview { get; set; }

        string Link { get; set; }

        bool Watched { get; set; }

        string Screen { get; set; }
        DateTime AirDate { get; set; }

        ObservableCollection<TraktComment> Comments { get; set; }
    }
}

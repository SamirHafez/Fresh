using System;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Interfaces
{
    public interface IEpisodeViewModel
    {
        int Number { get; set; }
        int Season { get; set; }
        string Screen { get; set; }
        DateTime FirstAired { get; set; }
        string Overview { get; set; }

        ObservableCollection<string> Links { get; set; }
    }
}

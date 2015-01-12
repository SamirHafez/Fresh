using Fresh.Windows.Core.Models;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface ISeasonPageViewModel
    { 
        int Number { get; set; }
        string Poster { get; set; }
        string Overview { get; set; }
        ObservableCollection<TraktEpisode> Episodes { get; set; } 
    }
}

using Fresh.Windows.Shared.Models;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface IMainPageViewModel
    {
        ObservableCollection<TVShow> Library { get; set; }
    }
}

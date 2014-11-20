using Fresh.Windows.Models;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Interfaces
{
    public interface IMainPageViewModel
    {
        ObservableCollection<TVShow> Collection { get; set; }
    }
}

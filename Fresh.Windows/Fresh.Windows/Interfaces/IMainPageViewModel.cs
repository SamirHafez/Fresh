using System.Collections.ObjectModel;

namespace Fresh.Windows.Interfaces
{
    public interface IMainPageViewModel
    {
        ObservableCollection<ITVShowViewModel> Collection { get; set; }
    }
}

using System.Collections.ObjectModel;

namespace Fresh.Windows.Interfaces
{
    public interface IMainPageViewModel
    {
        ObservableCollection<ITVShowPageViewModel> Collection { get; set; }
    }
}

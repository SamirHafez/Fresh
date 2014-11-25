using System.Collections.ObjectModel;

namespace Fresh.Windows.Interfaces
{
    public interface ISeasonPageViewModel
    { 
        int Number { get; set; }
        ObservableCollection<IEpisodeViewModel> Episodes { get; set; }
    }
}

using Fresh.Windows.Shared.Models;
using System.Collections.ObjectModel;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface ISeasonPageViewModel
    { 
        int Number { get; set; }
        ObservableCollection<Episode> Episodes { get; set; } 
    }
}

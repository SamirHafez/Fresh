using Fresh.Windows.Shared.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace Fresh.Windows.ViewModels
{
    public class EpisodeViewModel : ViewModel, IEpisodeViewModel
    {
        int season = default(int);
        public int Season { get { return season; } set { SetProperty(ref season, value); } }

        int number = default(int);
        public int Number { get { return number; } set { SetProperty(ref number, value); } }

        string screen = default(string);
        public string Screen { get { return screen; } set { SetProperty(ref screen, value); } }

        string overview = default(string);
        public string Overview { get { return overview; } set { SetProperty(ref overview, value); } }

        DateTime firstAired = default(DateTime);
        public DateTime FirstAired { get { return firstAired; } set { SetProperty(ref firstAired, value); } }

        ObservableCollection<string> links = default(ObservableCollection<string>);
        public ObservableCollection<string> Links { get { return links; } set { SetProperty(ref links, value); } }
    }
}

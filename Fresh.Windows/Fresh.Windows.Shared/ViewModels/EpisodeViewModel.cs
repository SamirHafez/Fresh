using Fresh.Windows.Shared.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using System;

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

        string link = default(string);
        public string Link { get { return link; } set { SetProperty(ref link, value); } }
    }
}

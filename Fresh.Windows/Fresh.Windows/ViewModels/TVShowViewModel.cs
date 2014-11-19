using Fresh.Windows.Interfaces;
using Microsoft.Practices.Prism.Mvvm;

namespace Fresh.Windows.ViewModels
{
    public class TVShowViewModel : ViewModel, ITVShowViewModel
    {
        string title = default(string);
        public string Title { get { return title; } set { SetProperty(ref title, value); } }

        int year = default(int);
        public int Year { get { return year; } set { SetProperty(ref year, value); } }

        string poster = default(string);
        public string Poster { get { return poster; } set { SetProperty(ref poster, value); } }
    }
}

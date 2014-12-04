using Fresh.Windows.Core.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.ComponentModel;

namespace Fresh.Windows.Shared.Models
{
    public class Episode : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Season))]
        public int SeasonId { get; set; }

        public string Title { get; set; }
        public int Number { get; set; }
        public string Overview { get; set; }
        public string Screen { get; set; }

        private string link;
        public string Link
        {
            get { return link; }
            set
            {
                link = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Link"));
            }
        }

        [ManyToOne]
        public Season Season { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static Episode FromTrakt(TraktEpisode trakt)
        {
            return new Episode
            {
                Title = trakt.Title,
                Number = trakt.Number,
                Overview = trakt.Overview,
                Screen = trakt.Screen
            };
        }
    }
}

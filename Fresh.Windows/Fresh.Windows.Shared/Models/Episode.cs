using Fresh.Windows.Core.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.ComponentModel;
using System.Globalization;

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

        public DateTime FirstAired { get; set; }

        [ManyToOne]
        public Season Season { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static Episode FromTrakt(TraktEpisode trakt)
        {
            return new Episode
            {
                Title = trakt.Title,
                Number = trakt.Number,
                FirstAired = DateTime.Parse(trakt.First_aired_iso ?? "1900-01-01", CultureInfo.InvariantCulture),
                Overview = trakt.Overview,
                Screen = trakt.Screen
            };
        }
    }
}

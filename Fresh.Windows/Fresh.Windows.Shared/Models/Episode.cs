using Fresh.Windows.Core.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Fresh.Windows.Shared.Models
{
    public class Episode : INotifyPropertyChanged
    {
        [PrimaryKey]
        public int Id { get; set; }

        [ForeignKey(typeof(Season))]
        public int SeasonId { get; set; }

        public string Title { get; set; }
        public int Number { get; set; }
        public string Overview { get; set; }
        public string Screen { get; set; }
        private DateTime? airDate;
        public DateTime? AirDate 
        {
            get { return airDate != null ? DateTime.SpecifyKind(airDate.Value, DateTimeKind.Utc) : (DateTime?)null; }
            set { airDate = value; }
        }

        private bool watched;
        public bool Watched
        {
            get { return watched; }
            set
            {
                watched = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Watched"));
            }
        }

        public string Link { get; set; }

        [ManyToOne]
        public Season Season { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static Episode FromTrakt(TraktEpisode trakt)
        {
            return new Episode
            {
                Id = trakt.Ids.Tvdb,
                Title = trakt.Title,
                Number = trakt.Number,
                Overview = trakt.Overview,
                Screen = trakt.Images.Screenshot.Full,
                AirDate = DateTime.Parse(trakt.First_Aired)
            };
        }
    }

    public class GroupedEpisodes<T>
    {
        public T Key { get; set; }
        public IList<Episode> Episodes { get; set; }
    }
}

using Fresh.Windows.Core.Models;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Fresh.Windows.Shared.Models
{
    public class Episode : INotifyPropertyChanged
    {
        [PrimaryKey]
        public int Id { get; set; }

        [ForeignKey(typeof(TVShow))]
        public int ShowId { get; set; }
        public int SeasonNumber { get; set; }

        public string Title { get; set; }
        public int Number { get; set; }
        public string Overview { get; set; }
        public string Screen { get; set; }
        public DateTime? AirDate { get; set; }
        public DateTime? LastUpdated { get; set; }

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
        public TVShow TVShow { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static Episode FromTrakt(TraktEpisode trakt, int showId)
        {
            return new Episode
            {
                Id = trakt.Ids.Trakt,
                ShowId = showId,
                Title = trakt.Title,
                Number = trakt.Number,
                SeasonNumber = trakt.Season,
                Overview = trakt.Overview,
                Screen = trakt.Images != null && trakt.Images.Screenshot != null ? trakt.Images.Screenshot.Full : null,
                AirDate = !string.IsNullOrWhiteSpace(trakt.First_Aired) ? DateTime.Parse(trakt.First_Aired, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal) : (DateTime?)null
            };
        }

        public void Update(TraktEpisode trakt)
        {
            Title = trakt.Title;
            Number = trakt.Number;
            SeasonNumber = trakt.Season;
            Overview = trakt.Overview;
            Screen = trakt.Images.Screenshot.Full;
            AirDate = !string.IsNullOrWhiteSpace(trakt.First_Aired) ? DateTime.Parse(trakt.First_Aired, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal) : (DateTime?)null;
            LastUpdated = !string.IsNullOrWhiteSpace(trakt.Updated_At) ? DateTime.Parse(trakt.Updated_At, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal) : (DateTime?)null;
        }
    }

    public class GroupedEpisodes<T>
    {
        public T Key { get; set; }
        public IList<Episode> Episodes { get; set; }
    }
}

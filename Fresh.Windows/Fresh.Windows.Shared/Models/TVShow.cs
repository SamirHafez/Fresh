using Fresh.Windows.Core.Models;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;

namespace Fresh.Windows.Shared.Models
{
    public class TVShow
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string Overview { get; set; }
        public string Network { get; set; }
        public string Poster { get; set; }
        public DayOfWeek? AirDay { get; set; }
        public double Rating { get; set; }
        public DateTime? LastUpdated { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Season> Seasons { get; set; }

        public static TVShow FromTrakt(TraktTVShow trakt)
        {
            return new TVShow
            {
                Id = trakt.Ids.Trakt,
                Title = trakt.Title,
                Year = trakt.Year,
                Overview = trakt.Overview,
                Network = trakt.Network,
                Rating = trakt.Rating,
                Poster = trakt.Images != null && trakt.Images.Poster != null ? trakt.Images.Poster.Full : null,
                AirDay = trakt.Airs != null && !string.IsNullOrWhiteSpace(trakt.Airs.Day) && trakt.Airs.Day != "Daily" ? (DayOfWeek)Enum.Parse(typeof(DayOfWeek), trakt.Airs.Day, ignoreCase: true) : (DayOfWeek?)null,
                Seasons = new List<Season>()
            };
        }

        public void UpdateWatched(TraktWatchedShow watchedShow)
        {
            foreach (var episode in from watchedSeason in watchedShow.Seasons
                                    from watchedEpisode in watchedSeason.Episodes
                                    from season in Seasons
                                    where season.Number == watchedSeason.Number
                                    from episode in season.Episodes
                                    where episode.Number == watchedEpisode.Number
                                    where episode.Watched == false
                                    select episode)
                episode.Watched = true;
        }

        public async Task UpdateAsync(ITraktService traktService)
        {
            var show = await traktService.GetShowAsync(Id, extended: TraktExtendEnum.FULL);
            var traktLastUpdated = DateTime.Parse(show.Updated_At, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

            if (traktLastUpdated == LastUpdated)
                return;

            var traktSeasons = await traktService.GetSeasonsAsync(Id, extended: TraktExtendEnum.FULL_IMAGES);

            foreach (var traktSeason in traktSeasons)
            {
                var season = (from s in Seasons
                              where s.Number == traktSeason.Number
                              select s).FirstOrDefault();

                if (season == null)
                {
                    var newSeason = Season.FromTrakt(traktSeason);
                    newSeason.TVShowId = Id;
                    await newSeason.UpdateAsync(traktService);
                    Seasons.Add(newSeason);
                }
                else if (season.Episodes.Count < traktSeason.Episode_Count)
                    await season.UpdateAsync(traktService);
            }

            LastUpdated = traktLastUpdated;
        }
    }
}

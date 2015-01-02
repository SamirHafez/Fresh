using Fresh.Windows.Core.Models;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fresh.Windows.Shared.Models
{
    public class TVShow
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Overview { get; set; }
        public string Network { get; set; }
        public string Poster { get; set; }
        public DayOfWeek? AirDay { get; set; }
        public double Rating { get; set; }

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
                Poster = trakt.Images.Poster.Full,
                AirDay = !string.IsNullOrWhiteSpace(trakt.Airs.Day) && trakt.Airs.Day != "Daily" ? (DayOfWeek)Enum.Parse(typeof(DayOfWeek), trakt.Airs.Day, ignoreCase: true) : (DayOfWeek?)null,
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
            var latestSeason = (from season in Seasons
                                orderby season.Number descending
                                select season).First();

            var traktLatestSeasonEpisodes = await traktService.GetSeasonEpisodesAsync(Id, latestSeason.Number);

            bool hasNewEpisodes = false;
            foreach (var traktEpisode in from ep in traktLatestSeasonEpisodes
                                         select Episode.FromTrakt(ep))
            {
                var episode = (from ep in latestSeason.Episodes
                               where ep.Number == traktEpisode.Number
                               select ep).FirstOrDefault();

                if (episode == null)
                {
                    hasNewEpisodes = true;
                    latestSeason.Episodes.Add(traktEpisode);
                }
                else
                {
                    episode.Title = traktEpisode.Title;
                    episode.Overview = traktEpisode.Overview;
                    episode.Screen = traktEpisode.Screen;
                    episode.AirDate = traktEpisode.AirDate;
                }
            }

            if (!hasNewEpisodes)
            {
                //var traktNextSeasonEpisodes = from episode in await traktService.GetSeasonEpisodesAsync(fullShow.Id, latestSeason.Number + 1, extended: TraktExtendEnum.FULL_IMAGES)
                //                              select Episode.FromTrakt(episode);
                //fullShow.Seasons.Add()
                //fullShow.Episodes.AddRange(traktNextSeasonEpisodes);
            }
        }
    }
}

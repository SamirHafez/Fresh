using Fresh.Windows.Core.Models;
using Fresh.Windows.Core.Services.Interfaces;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Fresh.Windows.Shared.Models
{
    public class Season
    {
        [PrimaryKey]
        public int Id { get; set; }

        [ForeignKey(typeof(TVShow))]
        public int TVShowId { get; set; }

        public int Number { get; set; }

        public double Rating { get; set; }
        public string Overview { get; set; }
        public string Poster { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Episode> Episodes { get; set; }

        [ManyToOne]
        public TVShow TVShow { get; set; }

        public static Season FromTrakt(TraktSeason trakt)
        {
            return new Season
            {
                Id = trakt.Ids.Tvdb ?? trakt.Ids.Trakt,
                Number = trakt.Number,
                Overview = trakt.Overview,
                Rating = trakt.Rating,
                Poster = trakt.Images.Poster.Full,
                Episodes = new List<Episode>()
            };
        }

        public async Task UpdateAsync(ITraktService traktService)
        {
            if (Episodes.Count == 0)
            {
                Episodes.AddRange(from traktEpisode in await traktService.GetSeasonEpisodesAsync(TVShowId, Number, extended: TraktExtendEnum.FULL_IMAGES)
                                  select Episode.FromTrakt(traktEpisode));
            }
            else 
                foreach (var traktEpisode in await traktService.GetSeasonEpisodesAsync(TVShowId, Number, extended: TraktExtendEnum.MIN))
                {
                    var episode = (from e in Episodes
                                   where e.Number == traktEpisode.Number
                                   select e).FirstOrDefault();

                    if (episode == null)
                    {
                        var fullTraktEpisode = await traktService.GetEpisodeAsync(TVShowId, Number, traktEpisode.Number, extended: TraktExtendEnum.FULL_IMAGES);
                        Episodes.Add(Episode.FromTrakt(fullTraktEpisode));
                    }
                    else if (traktEpisode.Updated_At != null && DateTime.Parse(traktEpisode.Updated_At) > episode.LastUpdated)
                        episode.Update(await traktService.GetEpisodeAsync(TVShowId, Number, traktEpisode.Number, extended: TraktExtendEnum.FULL_IMAGES));
                }
        }
    }
}

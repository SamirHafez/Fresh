using Fresh.Windows.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services.Interfaces
{
    public enum TraktExtendEnum
    {
        MIN,
        IMAGES,
        FULL,
        FULL_IMAGES,
        METADATA
    }

    public interface ITraktService
    {
        void SetAuthenticator(OAuthResponse oauthResponse);

        Task<OAuthResponse> LoginAsync(OAuthRequest oauthRequest);

        Task<TraktUser> GetSettingsAsync();

        Task<TraktTVShow> GetShowAsync(int showId, TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<IList<TraktSeason>> GetSeasonsAsync(int showId, TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<IList<TraktEpisode>> GetSeasonEpisodesAsync(int showId, int seasonNumber, TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<TraktEpisode> GetEpisodeAsync(int showId, int seasonNumber, int episodeNumber, TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<IList<TraktWatchedShow>> GetWatchedEpisodesAsync(TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task WatchEpisodesAsync(IList<int> episodeIds);

        Task<IList<TraktTVShow>> SearchTVShowAsync(string query);
    }
}

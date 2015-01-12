using Fresh.Windows.Core.Models;
using System;
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

        Task<IList<TraktComment>> GetShowCommentsAsync(int showId);

        Task<IList<TraktTVShow>> GetRelatedShowsAsync(int showId, TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<IList<TraktSeason>> GetSeasonsAsync(int showId, TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<IList<TraktEpisode>> GetSeasonEpisodesAsync(int showId, int seasonNumber, TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<TraktEpisode> GetEpisodeAsync(int showId, int seasonNumber, int episodeNumber, TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<IList<TraktComment>> GetEpisodeCommentsAsync(int showId, int seasonNumber, int episodeNumber);

        Task<IList<TraktWatchedShow>> GetWatchedEpisodesAsync(TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<TraktWatchedProgress> GetShowWatchedProgressAsync(int showId, TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<Dictionary<DateTime, List<TraktCalendarItem>>> GetCalendarAsync(DateTime startDate, int days, TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<IList<TraktTVShow>> GetRecommendedShowsAsync(TraktExtendEnum extended = TraktExtendEnum.MIN);

        Task<IList<TraktTVShow>> GetPopularShowsAsync(TraktExtendEnum extended = TraktExtendEnum.MIN, int page = 1, int limit = 10);

        Task<IList<TraktTrendingTVShow>> GetTrendingShowsAsync(TraktExtendEnum extended = TraktExtendEnum.MIN, int page = 1, int limit = 10);

        Task WatchEpisodesAsync(IList<int> episodeIds);

        Task<IList<TraktTVShowSearch>> SearchTVShowAsync(string query);

        Task<TraktActivity> GetLastActivityAsync();
    }
}

using Fresh.Windows.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services.Interfaces
{
    public interface ITraktService
    {
        Task LoginAsync(OAuthResponse oauthResponse);

        Task<OAuthResponse> LoginAsync(OAuthRequest oauthRequest);

        Task<dynamic> GetSettingsAsync(string username);

        Task<IList<TraktTVShow>> GetLibraryAsync(string username, bool extended = false);

        Task<TraktTVShow> GetShowAsync(string showId, bool extended = false);

        Task<IList<TraktEpisode>> GetSeasonEpisodesAsync(string showId, int seasonNumber, bool extended = false);

        Task<IList<TraktTVShow>> GetWatchedEpisodesAsync(string username);

        Task WatchEpisodesAsync(string username, string showTitle, int year, IList<dynamic> episodes);

        Task<IList<TraktTVShow>> SearchTVShowAsync(string query);

        Task AddShowToLibraryAsync(string username, string showTitle, int year);
    }
}

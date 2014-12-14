using Fresh.Windows.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services.Interfaces
{
    public interface ITraktService
    {
        Task<dynamic> GetSettingsAsync(string username, string password);

        Task<IList<TraktTVShow>> GetLibraryAsync(string username, bool extended = false);

        Task<TraktTVShow> GetShowAsync(string showId, bool extended = false);

        Task<IList<TraktEpisode>> GetSeasonEpisodesAsync(string showId, int seasonNumber, bool extended = false);

        Task<IList<TraktWatchedEpisode>> GetWatchedEpisodesAsync(string username);
    }
}

using Fresh.Windows.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fresh.Windows.Shared.Services.Interfaces
{
    public interface IStorageService : IDisposable
    {
        Task<User> GetUserAsync();

        Task<User> CreateOrUpdateUserAsync(User user);

        Task<IList<TVShow>> GetLibraryAsync();

        Task<TVShow> GetShowAsync(string showId);

        Task<Episode> GetEpisodeAsync(int episodeId);

        Task<IList<Episode>> GetEpisodesAsync(Expression<Func<Episode, bool>> predicate = null);

        Task UpdateLibraryAsync(IList<TVShow> library);

        Task UpdateShowAsync(TVShow dbShow);

        Task<Season> GetSeasonAsync(string showId, int seasonNumber);

        Task<Season> GetSeasonAsync(int seasonId);

        Task UpdateEpisodeAsync(Episode episode);
    }
}

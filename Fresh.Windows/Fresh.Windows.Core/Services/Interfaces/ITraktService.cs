﻿using Fresh.Windows.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services.Interfaces
{
    public interface ITraktService
    {
        Task<dynamic> GetSettings(string username, string password);

        Task<IList<TraktTVShow>> GetLibrary(string username, bool extended = false);

        Task<IList<TraktTVShow>> GetWatched(string username, bool extended = false);

        Task<TraktTVShow> GetShow(string showId, bool extended = false);

        Task<IList<TraktEpisode>> GetSeason(string showId, int seasonNumber, bool extended = false);
    }
}

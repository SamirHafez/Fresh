using System.Threading.Tasks;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using System.Collections.Generic;
using SQLite.Net.Async;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLiteNetExtensions.Extensions;
using System;

namespace Fresh.Windows.Shared.Services
{
    public class SQLiteService : IStorageService
    {
        private readonly SQLiteAsyncConnection context;
        private readonly SQLiteConnectionWithLock connection;

        private bool isDisposed;

        public SQLiteService(ISQLitePlatform platform)
        {
            connection = new SQLiteConnectionWithLock(platform, new SQLiteConnectionString("fresh.db", storeDateTimeAsTicks: false));
            context = new SQLiteAsyncConnection(() => connection);

            isDisposed = false;
        }

        public async Task<User> CreateOrUpdateUserAsync(User user)
        {
            await context.CreateTableAsync<User>();

            await context.InsertAsync(user);

            return user;
        }

        public async Task<IList<TVShow>> GetLibraryAsync()
        {
            await context.CreateTablesAsync<TVShow, Season, Episode>();

            return await context.Table<TVShow>().ToListAsync();
        }

        public async Task UpdateLibraryAsync(IList<TVShow> library)
        {
            var updated = await context.UpdateAllAsync(library);
            var inserted = await context.InsertAllAsync(library);
        }

        public async Task<User> GetUserAsync()
        {
            await context.CreateTableAsync<User>();

            return await context.Table<User>().
                FirstOrDefaultAsync();
        }

        public async Task<TVShow> GetShowAsync(string showId)
        {
            await context.CreateTablesAsync<TVShow, Season, Episode>();

            return connection.GetWithChildren<TVShow>(showId);
        }

        public async Task UpdateShowAsync(TVShow dbShow)
        {
            await context.CreateTablesAsync<TVShow, Season, Episode>();

            connection.InsertOrReplaceWithChildren(dbShow, recursive: true);
        }

        public async Task<Season> GetSeasonAsync(string showId, int seasonNumber)
        {
            await context.CreateTablesAsync<TVShow, Season, Episode>();

            var season = await context.Table<Season>().
                Where(s => s.ShowId == showId && s.Number == seasonNumber)
                .FirstAsync();

            return await GetSeasonAsync(season.Id);
        }

        public async Task<Season> GetSeasonAsync(int seasonId)
        {
            await context.CreateTablesAsync<TVShow, Season, Episode>();

            return connection.GetWithChildren<Season>(seasonId, recursive: true);
        }

        public async Task UpdateEpisodeAsync(Episode episode)
        {
            await context.CreateTablesAsync<TVShow, Season, Episode>();

            var updated = await context.UpdateAsync(episode);
        }

        public async Task<Episode> GetEpisodeAsync(int episodeId)
        {
            await context.CreateTablesAsync<TVShow, Season, Episode>();

            var episode = connection.GetWithChildren<Episode>(episodeId, recursive: true);

            connection.GetChildren(episode.Season, recursive: false);

            return episode;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
            {
                if (connection.IsInTransaction)
                    connection.Rollback();

                using (connection.Lock())
                    connection.Dispose();
            }

            isDisposed = true;
        }
    }
}

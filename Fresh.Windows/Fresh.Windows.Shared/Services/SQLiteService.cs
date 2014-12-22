using System.Threading.Tasks;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using System.Collections.Generic;
using SQLite.Net.Async;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLiteNetExtensions.Extensions;
using System;
using System.Linq.Expressions;

namespace Fresh.Windows.Shared.Services
{
    public class SQLiteService : IStorageService
    {
        private readonly SQLiteAsyncConnection context;
        private readonly SQLiteConnectionWithLock connection;

        private bool isDisposed;

        public SQLiteService(ISQLitePlatform platform, SQLiteConnectionString connectionString)
        {
            connection = new SQLiteConnectionWithLock(platform, connectionString);
            context = new SQLiteAsyncConnection(() => connection);
            isDisposed = false;
        }

        public async Task<User> CreateOrUpdateUserAsync(User user)
        {
            await context.CreateTableAsync<User>();

            await context.InsertOrReplaceAsync(user);

            return user;
        }

        public async Task<IList<TVShow>> GetLibraryAsync()
        {
            //await context.DropTableAsync<TVShow>();
            //await context.DropTableAsync<Season>();
            //await context.DropTableAsync<Episode>();
            //await context.DropTableAsync<User>();

            await context.CreateTablesAsync<TVShow, Season, Episode>();

            return await context.Table<TVShow>().ToListAsync();
        }

        public async Task UpdateLibraryAsync(IList<TVShow> library)
        { 
            await context.CreateTablesAsync<TVShow, Season, Episode>();

            connection.InsertOrReplaceAllWithChildren(library, recursive: true);
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

            var show = connection.GetWithChildren<TVShow>(showId);

            foreach (var season in show.Seasons)
                connection.GetChildren(season, recursive: true);

            return show;
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

            await context.InsertOrReplaceAsync(episode);
        }

        public async Task<Episode> GetEpisodeAsync(int episodeId)
        {
            await context.CreateTablesAsync<TVShow, Season, Episode>();

            var episode = connection.GetWithChildren<Episode>(episodeId, recursive: true);

            connection.GetChildren(episode.Season, recursive: false);

            return episode;
        }

        public async Task<IList<Episode>> GetEpisodesAsync(Expression<Func<Episode, bool>> predicate = null)
        {
            await context.CreateTablesAsync<TVShow, Season, Episode>();

            var episodes = connection.GetAllWithChildren(predicate, recursive: true);

            foreach (var episode in episodes)
                connection.GetChildren(episode.Season);

            return episodes;
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

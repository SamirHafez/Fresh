﻿using System.Threading.Tasks;
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

        private static object _lock = new Object();

        public SQLiteService(ISQLitePlatform platform, SQLiteConnectionString connectionString)
        {
            connection = new SQLiteConnectionWithLock(platform, connectionString);
            context = new SQLiteAsyncConnection(() => connection);
            isDisposed = false;
        }

        public async Task<User> CreateOrUpdateUserAsync(User user)
        {
            await context.CreateTablesAsync<User, TVShow, Season, Episode>();

            await context.InsertOrReplaceAsync(user);

            return user;
        }

        public Task<List<TVShow>> GetLibraryAsync()
        {
            return context.Table<TVShow>().ToListAsync();
        }

        public Task UpdateLibraryAsync(IList<TVShow> library)
        {
            return Task.Run(delegate
            {
                lock (_lock)
                    connection.InsertOrReplaceAllWithChildren(library, recursive: true);
            });
        }

        public async Task<User> GetUserAsync()
        {
            await context.CreateTablesAsync<User, TVShow, Season, Episode>();

            return await context.Table<User>().
                FirstOrDefaultAsync();
        }

        public Task<TVShow> GetShowAsync(string showId)
        {
            return Task.Run<TVShow>(delegate
            {
                try
                {
                    lock (_lock)
                    {
                        var show = connection.GetWithChildren<TVShow>(showId);

                        foreach (var season in show.Seasons)
                            connection.GetChildren(season, recursive: true);

                        return show;
                    }
                }
                catch
                {
                    return null;
                }
            });

        }

        public Task UpdateShowAsync(TVShow dbShow)
        {
            return Task.Run(delegate
            {
                lock (_lock)
                    connection.InsertOrReplaceWithChildren(dbShow, recursive: true);
            });
        }

        public async Task<Season> GetSeasonAsync(string showId, int seasonNumber)
        {
            var season = await context.Table<Season>().
                Where(s => s.ShowId == showId && s.Number == seasonNumber).
                FirstAsync();

            return await GetSeasonAsync(season.Id);
        }

        public Task<Season> GetSeasonAsync(int seasonId)
        {
            return Task.Run(delegate
            {
                lock (_lock)
                    return connection.GetWithChildren<Season>(seasonId, recursive: true);
            });

        }

        public Task UpdateEpisodeAsync(Episode episode)
        {
            return context.InsertOrReplaceAsync(episode);
        }

        public Task<Episode> GetEpisodeAsync(int episodeId)
        {
            return Task.Run<Episode>(delegate
            {
                lock (_lock)
                {
                    var episode = connection.GetWithChildren<Episode>(episodeId, recursive: true);

                    connection.GetChildren(episode.Season, recursive: false);

                    return episode;
                }
            });

        }

        public Task<IList<Episode>> GetEpisodesAsync(Expression<Func<Episode, bool>> predicate = null)
        {
            return Task.Run<IList<Episode>>(delegate
            {
                lock (_lock)
                {
                    var episodes = connection.GetAllWithChildren(predicate, recursive: true);

                    foreach (var episode in episodes)
                        connection.GetChildren(episode.Season);

                    return episodes;
                }
            });

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

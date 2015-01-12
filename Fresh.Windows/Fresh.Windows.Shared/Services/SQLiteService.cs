using System.Threading.Tasks;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using SQLite.Net.Async;
using SQLite.Net;
using SQLite.Net.Interop;
using System;

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
            await context.CreateTableAsync<User>();

            await context.InsertOrReplaceAsync(user);

            return user;
        }

        public async Task<User> GetUserAsync()
        {
            await context.CreateTableAsync<User>();

            return await context.Table<User>().
                FirstOrDefaultAsync();
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

using Fresh.Windows.Core.Models;
using Fresh.Windows.Core.Services.Interfaces;
using SQLite;
using System;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services
{
    public class SQLiteService : IStorageService
    {
        public readonly SQLiteAsyncConnection context;

        public SQLiteService()
        {
            context = new SQLiteAsyncConnection("fresh.db");
        }

        public Task<T> Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserAsync()
        {
            await context.CreateTableAsync<User>();

            return await context.Table<User>().
                FirstOrDefaultAsync();
        }

        public Task<bool> HasKey(string key)
        {
            throw new NotImplementedException();
        }

        public Task Save<T>(string key, T entity)
        {
            throw new NotImplementedException();
        }
    }
}

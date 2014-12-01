using System.Threading.Tasks;
using SQLite;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Fresh.Windows.Shared.Services
{
    public class SQLiteService : IStorageService
    {
        private readonly SQLiteAsyncConnection context;

        public SQLiteService()
        {
            context = new SQLiteAsyncConnection("fresh.db", storeDateTimeAsTicks: false);
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

        public Task UpdateLibraryAsync(IList<TVShow> library)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserAsync()
        {
            await context.CreateTableAsync<User>();

            return await context.Table<User>().
                FirstOrDefaultAsync();
        }
    }
}

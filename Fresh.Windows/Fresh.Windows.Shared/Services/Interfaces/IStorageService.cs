using Fresh.Windows.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Fresh.Windows.Shared.Services.Interfaces
{
    public interface IStorageService : IDisposable
    {
        Task<User> GetUserAsync();

        Task<User> CreateOrUpdateUserAsync(User user);
    }
}

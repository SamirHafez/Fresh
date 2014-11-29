using Fresh.Windows.Core.Models;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services.Interfaces
{
    public interface IStorageService
    {
        Task<User> GetUserAsync();

        Task<bool> HasKey(string key);

        Task Save<T>(string key, T entity);

        Task<T> Get<T>(string key);
    }
}

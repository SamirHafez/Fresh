using Fresh.Windows.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fresh.Windows.Shared.Services.Interfaces
{
    public interface IStorageService
    {
        Task<User> GetUserAsync();

        Task<User> CreateOrUpdateUserAsync(User user);

        Task<IList<TVShow>> GetLibraryAsync();

        Task UpdateLibraryAsync(IList<TVShow> library);
    }
}

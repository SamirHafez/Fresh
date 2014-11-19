using System;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services.Interfaces
{
    public interface ILoginService
    {
        event EventHandler StateChanged;

        Task<bool> SilentLoginAsync();

        Task LoginAsync(string username, string password);

        Task LogoutAsync();
    }
}

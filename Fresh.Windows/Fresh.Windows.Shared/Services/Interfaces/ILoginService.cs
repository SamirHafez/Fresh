using System.Threading.Tasks;

namespace Fresh.Windows.Shared.Services.Interfaces
{
    public interface ILoginService
    {
        Task<bool> SilentLoginAsync();

        Task LoginAsync(string username, string password);

        Task LogoutAsync();
    }
}

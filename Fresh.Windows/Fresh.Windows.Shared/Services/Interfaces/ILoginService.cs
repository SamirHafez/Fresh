using Fresh.Windows.Core.Services;
using System.Threading.Tasks;

namespace Fresh.Windows.Shared.Services.Interfaces
{
    public interface ILoginService
    {
        Task<bool> SilentLoginAsync();

        Task LoginAsync(OAuthRequest oauthRequest);

        Task LogoutAsync();
    }
}

using System;
using System.Threading.Tasks;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using Fresh.Windows.Core.Services;
using Fresh.Windows.Shared.Models;

namespace Fresh.Windows.Shared.Services
{
    public class LoginService : ILoginService
    {
        private readonly IStorageService storageService;
        private readonly ITraktService traktService;

        public LoginService(IStorageService storageService, ITraktService traktService)
        {
            this.storageService = storageService;
            this.traktService = traktService;
        }

        public async Task LoginAsync(OAuthRequest oauthRequest)
        {
            var response = await traktService.LoginAsync(oauthRequest);

            var userSettings = await traktService.GetSettingsAsync();

            var user = new User
            {
                Username = userSettings.User.Username,
                AccessToken = response.Access_Token,
                Refresh_Token = response.Refresh_Token
            };

            await storageService.CreateOrUpdateUserAsync(user);
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SilentLoginAsync()
        {
            var user = await storageService.GetUserAsync();

            if (user != null)
            {
                traktService.SetAuthenticator(new OAuthResponse
                {
                    Access_Token = user.AccessToken
                });
            }

            return user != null;
        }
    }
}

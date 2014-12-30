using System;
using System.Threading.Tasks;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using Fresh.Windows.Shared.Configuration;
using Fresh.Windows.Core.Services;
using Fresh.Windows.Shared.Models;

namespace Fresh.Windows.Shared.Services
{
    public class LoginService : ILoginService
    {
        private readonly IStorageService storageService;
        private readonly ITraktService traktService;
        private readonly ISession session;

        public LoginService(IStorageService storageService, ITraktService traktService, ISession session)
        {
            this.storageService = storageService;
            this.traktService = traktService;
            this.session = session;
        }

        public async Task LoginAsync(OAuthRequest oauthRequest)
        {
            var response = await traktService.LoginAsync(oauthRequest);

            var user = new User
            {
                Username = string.Empty,
                AccessToken = response.Access_Token,
                Refresh_Token = response.Refresh_Token
            };

            await storageService.CreateOrUpdateUserAsync(user);

            session.User = user;
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
                session.User = user;
                await traktService.LoginAsync(new OAuthResponse
                {
                    Access_Token = user.AccessToken
                });
            }

            return user != null;
        }
    }
}

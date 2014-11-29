using System;
using System.Threading.Tasks;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Core.Configuration;

namespace Fresh.Windows.Core.Services
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

        public async Task LoginAsync(string username, string password)
        {
            try
            {
                dynamic settings = await traktService.GetSettings(username, password);
                session.User = settings["username"].Value;
                storageService.Save("user", settings);
            }
            catch (Exception exception)
            {
                throw new AggregateException("Login failed.", exception);
            }
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SilentLoginAsync()
        {
            var user = await storageService.GetUserAsync();

            if (user != null)
                session.User = user;

            return user != null;
        }
    }
}

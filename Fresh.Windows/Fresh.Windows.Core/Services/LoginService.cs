using System;
using System.Threading.Tasks;
using Fresh.Windows.Core.Services.Interfaces;

namespace Fresh.Windows.Core.Services
{
    public class LoginService : ILoginService
    {
        public event EventHandler StateChanged;

        private readonly IStorageService storageService;
        private readonly ITraktService traktService;
        private readonly IConfigurationService configurationService;

        public LoginService(IStorageService storageService, ITraktService traktService, IConfigurationService configurationService)
        {
            this.storageService = storageService;
            this.traktService = traktService;
            this.configurationService = configurationService;
        }

        public async Task LoginAsync(string username, string password)
        {
            if (StateChanged != null)
                StateChanged(this, EventArgs.Empty);

            try
            {
                dynamic settings = await traktService.GetSettings(username, password);
                configurationService.Username = settings["username"].Value;
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
            var hasKey = await storageService.HasKey("user");

            if (hasKey)
            {
                dynamic settings = await storageService.Get<dynamic>("user");
                configurationService.Username = settings["username"].Value;
            }

            return hasKey;
        }
    }
}

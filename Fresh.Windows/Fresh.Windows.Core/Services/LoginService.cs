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

        public LoginService(IStorageService storageService, ITraktService traktService)
        {
            this.storageService = storageService;
            this.traktService = traktService;
        }

        public async Task LoginAsync(string username, string password)
        {
            if (StateChanged != null)
                StateChanged(this, EventArgs.Empty);

            try
            {
                dynamic settings = await traktService.GetSettings(username, password);
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

        public Task<bool> SilentLoginAsync()
        {
            return storageService.HasKey("user");
        }
    }
}

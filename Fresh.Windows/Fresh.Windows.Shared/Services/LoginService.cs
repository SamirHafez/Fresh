using System;
using System.Threading.Tasks;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using Fresh.Windows.Shared.Configuration;
using Fresh.Windows.Shared.Models;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

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

        public async Task LoginAsync(string username, string password)
        {
            try
            {
                var passVector = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
                var digest = HashAlgorithmProvider.OpenAlgorithm("SHA1").HashData(passVector);

                password = CryptographicBuffer.EncodeToHexString(digest);

                dynamic settings = await traktService.GetSettingsAsync(username, password);
                var user = new User { Username = settings["username"].Value, Credential = password };
                await storageService.CreateOrUpdateUserAsync(user);

                session.User = user;
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

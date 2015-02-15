using System.Threading.Tasks;
using Fresh.Core.Services.Interfaces;
using Newtonsoft.Json;
using System;
using Cirrious.MvvmCross.Plugins.File;

namespace Fresh.Core.Services
{
    public class LoginService : ILoginService
    {
        OAuthToken token;
        IMvxFileStore fileStore;

        public OAuthToken Token { get { return token; } }

        public readonly string ClientId;
        public readonly string ClientSecret;
        public readonly string TokenPath;
        public readonly Uri AuthorizeUri;
        public readonly Uri RedirectUri;
        public readonly Uri AccessUri;

        public LoginService(string tokenPath, string clientId, string clientSecret, Uri authorizeUri, Uri redirectUri, Uri accessUri, IMvxFileStore fileStore)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            TokenPath = tokenPath;
            AuthorizeUri = authorizeUri;
            RedirectUri = redirectUri;
            AccessUri = accessUri;

            this.fileStore = fileStore;
        }

        public Task<bool> SilentLoginAsync()
        {
            return Task.Run<bool>(delegate
            {
                if (!fileStore.Exists(TokenPath))
                    return false;

                string fileText;
                if (!fileStore.TryReadTextFile(TokenPath, out fileText))
                    return false;

                token = JsonConvert.DeserializeObject<OAuthToken>(fileText);

                return true;
            });
        }

        public async Task LauchLoginAsync(IOAuthBrokerService oauthBroker)
        {
            await oauthBroker.LaunchBrokerAsync(ClientId, ClientSecret, AuthorizeUri, RedirectUri, AccessUri);
        }

        public Task SaveAsync(OAuthToken token)
        {
            return Task.Run(delegate
            {
                var json = JsonConvert.SerializeObject(token);

                fileStore.WriteFile(TokenPath, json);
            });
        }
    }
}

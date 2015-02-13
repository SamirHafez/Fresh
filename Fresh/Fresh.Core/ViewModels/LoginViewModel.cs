using System;
using Cirrious.MvvmCross.ViewModels;
using Fresh.Core.Services.Interfaces;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using System.Net.Http;

namespace Fresh.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        IOAuthBrokerService oauthService;

        const string ClientId = "cad18e7e5ba53d5cd88652204a774d1e5c3bd69a20ca04899102d616df58f71f";
        const string ClientSecret = "9f2e811e93c05e29d61812146fa2e12dbf6cbf9caff639b1de23465b2e2817cb";
        static Uri AuthorizeUri = new Uri("https://trakt.tv/oauth/authorize");
        static Uri RedirectUri = new Uri("urn:ietf:wg:oauth:2.0:oob");
        static Uri AccessUri = new Uri("https://trakt.tv/oauth/token");

        string authCode;
        public string AuthCode { get { return authCode; } set { authCode = value; RaisePropertyChanged(() => AuthCode); doneCommand.RaiseCanExecuteChanged(); } }

        IMvxCommand doneCommand;
        public IMvxCommand DoneCommand
        {
            get { return doneCommand ?? (doneCommand = new MvxCommand(async () => await ContinueLogin(), () => !string.IsNullOrWhiteSpace(AuthCode))); }
        }

        public LoginViewModel(IOAuthBrokerService oauthService)
        {
            this.oauthService = oauthService;
        }

        protected override async void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);

            await TryLoginAsync();
        }

        private async Task TryLoginAsync()
        {
            await oauthService.LaunchBrokerAsync(ClientId, ClientSecret, AuthorizeUri, RedirectUri, AccessUri);
        }

        private async Task ContinueLogin()
        {
            var code = AuthCode;

            var client = new RestClient("https://api.trakt.tv");
            var authenticator = new TraktAuthenticator(ClientId, ClientSecret, code, RedirectUri);
            await authenticator.GetAccessTokenAsync(client);
            client.Authenticator = authenticator;
        }
    }

    public class TraktAuthenticator : IAuthenticator
    {
        string accessToken;
        string authCode;

        readonly string ClientId;
        readonly string ClientSecret;
        readonly Uri RedirectUri;

        public class OAuthResponse
        {
            public string Access_Token { get; set; }
            public string Token_type { get; set; }
            public int Expires_In { get; set; }
            public string Refresh_Token { get; set; }
            public string Scope { get; set; }
        }

        public TraktAuthenticator(string clientId, string clientSecret, string authCode, Uri redirectUri)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUri = redirectUri;
            this.authCode = authCode;
        }

        void IAuthenticator.Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddHeader("authorization", string.Format("Bearer {0}", accessToken)).
                    AddHeader("trakt-api-version", 2).
                    AddHeader("trakt-api-key", ClientId);
        }

        public async Task GetAccessTokenAsync(IRestClient client)
        {
            var request = new RestRequest("oauth/token", HttpMethod.Post).
                AddJsonBody(new
            {
                code = authCode,
                redirect_uri = RedirectUri.AbsoluteUri,
                grant_type = "authorization_code",
                client_id = ClientId,
                client_secret = ClientSecret
            });

            var response = await client.Execute<OAuthResponse>(request);

            accessToken = response.Data.Access_Token;
        }
    }
}
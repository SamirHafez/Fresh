using Fresh.Core.Models;
using Fresh.Core.Services.Interfaces;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fresh.Core.Services
{
    public enum TraktExtendEnum
    {
        MIN,
        IMAGES,
        FULL,
        FULL_IMAGES,
        METADATA
    }

    public class OAuthToken
    {
        public string Access_Token { get; set; }
        public string Token_type { get; set; }
        public int Expires_In { get; set; }
        public string Refresh_Token { get; set; }
        public string Scope { get; set; }
    }

    internal class TraktAuthenticator : IAuthenticator
    {
        OAuthToken token;
        public OAuthToken Token { get { return token; } }

        readonly string ClientId;
        readonly string ClientSecret;
        readonly Uri RedirectUri;

        public TraktAuthenticator(string clientId, string clientSecret, Uri redirectUri, OAuthToken token = null)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUri = redirectUri;
            this.token = token;
        }

        void IAuthenticator.Authenticate(IRestClient client, IRestRequest request)
        {
            if (request.Resource == "oauth/token")
                return;

            if (token == null)
                throw new Exception("Invalid token. Make sure to call GetAccessTokenAsync prior to your first api call.");

            request.AddHeader("authorization", string.Format("{0} {1}", "Bearer"/*token.Token_type*/, token.Access_Token)).
                    AddHeader("trakt-api-version", 2).
                    AddHeader("trakt-api-key", ClientId);
        }

        public async Task GetAccessTokenAsync(IRestClient client, string authCode)
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

            var response = await client.Execute<OAuthToken>(request);

            token = response.Data;
        }
    }

    public class TraktService : ITraktService
    {
        RestClient client;

        public TraktService(string clientId, string clientSecret, Uri redirectUri, OAuthToken token = null)
        {
            client = new RestClient("https://api.trakt.tv");
            client.Authenticator = new TraktAuthenticator(clientId, clientSecret, redirectUri, token);
        }

        public async Task<OAuthToken> SetAuthCodeAsync(string code)
        {
            var authenticator = (TraktAuthenticator)client.Authenticator;
            await authenticator.GetAccessTokenAsync(client, code);

            return authenticator.Token;
        }

        public async Task<IList<TVShow>> GetPopularShowsAsync(TraktExtendEnum extended = TraktExtendEnum.MIN, int page = 1, int limit = 10)
        {
            var request = new RestRequest("shows/popular").
                AddQueryParameter("page", page).
                AddQueryParameter("limit", limit);

            FillExtended(request, extended);

            var response = await client.Execute<IList<TVShow>>(request);

            return response.Data;
        }

        public async Task<Dictionary<DateTime, List<CalendarItem>>> GetCalendarAsync(DateTime startDate, int days, TraktExtendEnum extended = TraktExtendEnum.MIN)
        {
            var request = new RestRequest("calendars/shows/{start_date}/{days}").
                           AddUrlSegment("start_date", startDate.ToString("yyyy-MM-dd")).
                           AddUrlSegment("days", days);

            FillExtended(request, extended);

            var response = await client.Execute<Dictionary<DateTime, List<CalendarItem>>>(request);

            return response.Data;
        }

        private IRestRequest FillExtended(IRestRequest request, TraktExtendEnum extended)
        {
            string extendedText;
            switch (extended)
            {
            case TraktExtendEnum.MIN:
                extendedText = "min";
                break;
            case TraktExtendEnum.IMAGES:
                extendedText = "images";
                break;
            case TraktExtendEnum.FULL:
                extendedText = "full";
                break;
            case TraktExtendEnum.FULL_IMAGES:
                extendedText = "full,images";
                break;
            case TraktExtendEnum.METADATA:
                extendedText = "metadata";
                break;
            default:
                extendedText = "min";
                break;
            }

            return request.AddQueryParameter("extended", extendedText);
        }
    }
}

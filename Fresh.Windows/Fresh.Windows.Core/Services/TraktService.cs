using System;
using System.Threading.Tasks;
using Fresh.Windows.Core.Services.Interfaces;
using System.Net.Http;
using Fresh.Windows.Core.Models;
using System.Collections.Generic;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Fresh.Windows.Core.Services
{
    public class OAuthResponse
    {
        public string Access_Token { get; set; }
        public string Token_type { get; set; }
        public int Expires_In { get; set; }
        public string Refresh_Token { get; set; }
        public string Scope { get; set; }
    }

    public class OAuthRequest
    {
        public string code { get; set; }
        public string redirect_uri { get; set; }
        public string grant_type { get; set; }
    }

    internal class OAuthAuthenticator : IAuthenticator
    {
        private readonly string ClientId;
        private readonly string Token;

        public OAuthAuthenticator(string clientId, string token)
        {
            this.ClientId = clientId;
            this.Token = token;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddHeader("authorization", string.Format("Bearer {0}", Token)).
                AddHeader("trakt-api-version", 2).
                AddHeader("trakt-api-key", ClientId);
        }
    }

    public class TraktService : ITraktService
    {
        private readonly RestClient RestClient;

        private readonly string Client;
        private readonly string Secret;

        public TraktService(string client, string secret)
        {
            RestClient = new RestClient("https://api.trakt.tv");

            this.Client = client;
            this.Secret = secret;
        }

        public async Task<OAuthResponse> LoginAsync(OAuthRequest oauthRequest)
        {
            var request = new RestRequest("oauth/token", HttpMethod.Post).
                AddJsonBody(new
            {
                oauthRequest.code,
                oauthRequest.redirect_uri,
                oauthRequest.grant_type,
                client_id = Client,
                client_secret = Secret
            });

            var response = await RestClient.Execute<OAuthResponse>(request);

            SetAuthenticator(response.Data);

            return response.Data;
        }

        public void SetAuthenticator(OAuthResponse oauthResponse)
        {
            RestClient.Authenticator = new OAuthAuthenticator(this.Client, oauthResponse.Access_Token);
        }

        public async Task<TraktUser> GetSettingsAsync()
        {
            var request = new RestRequest("users/settings");

            var response = await RestClient.Execute<TraktUser>(request);

            return response.Data;
        }

        public async Task<TraktTVShow> GetShowAsync(int showId, TraktExtendEnum extended = TraktExtendEnum.MIN)
        {
            var request = new RestRequest("shows/{id}").
                AddUrlSegment("id", showId);

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<TraktTVShow>(request);

            return response.Data;
        }

        public async Task<IList<TraktComment>> GetShowCommentsAsync(int showId)
        {
            var request = new RestRequest("shows/{id}/comments").
                AddUrlSegment("id", showId);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<IList<TraktComment>>(request);

            return response.Data;
        }

        public async Task<IList<TraktTVShow>> GetRelatedShowsAsync(int showId, TraktExtendEnum extended = TraktExtendEnum.MIN)
        {
            var request = new RestRequest("shows/{id}/related").
                AddUrlSegment("id", showId);

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<IList<TraktTVShow>>(request);

            return response.Data;
        }

        public async Task<IList<TraktSeason>> GetSeasonsAsync(int showId, TraktExtendEnum extended = TraktExtendEnum.MIN)
        {
            var request = new RestRequest("shows/{id}/seasons").
                AddUrlSegment("id", showId);

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<IList<TraktSeason>>(request);

            return response.Data;
        }

        public async Task<IList<TraktEpisode>> GetSeasonEpisodesAsync(int showId, int seasonNumber, TraktExtendEnum extended = TraktExtendEnum.MIN)
        {
            var request = new RestRequest("shows/{id}/seasons/{season}").
                AddUrlSegment("id", showId).
                AddUrlSegment("season", seasonNumber);

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<IList<TraktEpisode>>(request);

            return response.Data;
        }

        public async Task<TraktEpisode> GetEpisodeAsync(int showId, int seasonNumber, int episodeNumber, TraktExtendEnum extended = TraktExtendEnum.MIN)
        {
            var request = new RestRequest("shows/{id}/seasons/{season}/episodes/{episode}").
                AddUrlSegment("id", showId).
                AddUrlSegment("season", seasonNumber).
                AddUrlSegment("episode", episodeNumber);

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<TraktEpisode>(request);

            return response.Data;
        }

        public async Task<IList<TraktComment>> GetEpisodeCommentsAsync(int showId, int seasonNumber, int episodeNumber)
        {
            var request = new RestRequest("shows/{id}/seasons/{season}/episodes/{episode}/comments").
                AddUrlSegment("id", showId).
                AddUrlSegment("season", seasonNumber).
                AddUrlSegment("episode", episodeNumber);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<IList<TraktComment>>(request);

            return response.Data;
        }

        public async Task<IList<TraktWatchedShow>> GetWatchedEpisodesAsync(TraktExtendEnum extended = TraktExtendEnum.MIN)
        {
            var request = new RestRequest("sync/watched/shows");

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<IList<TraktWatchedShow>>(request);

            return response.Data;
        }

        public async Task<TraktWatchedProgress> GetShowWatchedProgressAsync(int showId, TraktExtendEnum extended = TraktExtendEnum.MIN)
        {
            var request = new RestRequest("shows/{id}/progress/watched").
                AddUrlSegment("id", showId);

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<TraktWatchedProgress>(request);

            return response.Data;
        }

        public async Task<Dictionary<DateTime, List<TraktCalendarItem>>> GetCalendarAsync(DateTime startDate, int days, TraktExtendEnum extended = TraktExtendEnum.MIN)
        {
            var request = new RestRequest("calendars/shows/{start_date}/{days}").
                           AddUrlSegment("start_date", startDate.ToString("yyyy-MM-dd")).
                           AddUrlSegment("days", days);

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<Dictionary<DateTime, List<TraktCalendarItem>>>(request);

            return response.Data;
        }

        public async Task<IList<TraktTVShow>> GetRecommendedShowsAsync(TraktExtendEnum extended = TraktExtendEnum.MIN)
        {
            var request = new RestRequest("recommendations/shows");

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<IList<TraktTVShow>>(request);

            return response.Data;
        }

        public async Task<IList<TraktTVShow>> GetPopularShowsAsync(TraktExtendEnum extended = TraktExtendEnum.MIN, int page = 1, int limit = 10)
        {
            var request = new RestRequest("shows/popular").
                AddQueryParameter("page", page).
                AddQueryParameter("limit", limit);

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<IList<TraktTVShow>>(request);

            return response.Data;
        }

        public async Task<IList<TraktTrendingTVShow>> GetTrendingShowsAsync(TraktExtendEnum extended = TraktExtendEnum.MIN, int page = 1, int limit = 10)
        {
            var request = new RestRequest("shows/trending").
                AddQueryParameter("page", page).
                AddQueryParameter("limit", limit);

            FillExtended(request, extended);

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<IList<TraktTrendingTVShow>>(request);

            return response.Data;
        }

        public async Task<TraktActivity> GetLastActivityAsync()
        {
            var request = new RestRequest("sync/last_activities");

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<TraktActivity>(request);

            return response.Data;
        }

        public async Task<IList<TraktTVShowSearch>> SearchTVShowAsync(string query)
        {
            var request = new RestRequest("search").
                AddQueryParameter("query", query).
                AddQueryParameter("type", "show");

            Debug.WriteLine("Requesting {0}", request.Resource);

            var response = await RestClient.Execute<IList<TraktTVShowSearch>>(request);

            return response.Data;
        }

        public async Task WatchEpisodesAsync(IList<int> episodeIds)
        {
            var now = DateTime.UtcNow;
            var request = new RestRequest("sync/history", HttpMethod.Post).
                AddJsonBody(new
            {
                episodes = from episodeId in episodeIds
                           select new
                           {
                               watched_at = now,
                               ids = new
                               {
                                   trakt = episodeId
                               }
                           }
            });

            Debug.WriteLine("Posting {0}", request.Resource);

            var response = await RestClient.Execute(request);
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

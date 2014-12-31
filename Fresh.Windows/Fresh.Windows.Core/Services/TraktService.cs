﻿using System;
using System.Threading.Tasks;
using Fresh.Windows.Core.Services.Interfaces;
using System.Net.Http;
using Newtonsoft.Json;
using Fresh.Windows.Core.Models;
using System.Collections.Generic;
using System.Net;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;

namespace Fresh.Windows.Core.Services
{
    internal class TraktIO<T>
    {
        private const string TRAKT_API_URL = @"https://api.trakt.tv/";

        private bool asPost;
        private bool extended;
        private string path;
        private Func<dynamic, dynamic> withSelect;

        private dynamic parameters;

        public TraktIO<T> ForPath(string path)
        {
            this.path = path;
            return this;
        }

        public TraktIO<T> AsPost()
        {
            this.asPost = true;
            return this;
        }

        public TraktIO<T> WithParameters(dynamic parameters)
        {
            this.parameters = parameters;
            return this;
        }

        public TraktIO<T> Select(Func<dynamic, dynamic> withSelect)
        {
            this.withSelect = withSelect;
            return this;
        }

        public TraktIO<T> Extended(bool extended = true)
        {
            this.extended = extended;
            return this;
        }

        public async Task<T> Execute()
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
                response = asPost ?
                    await httpClient.PostAsync(GenerateQueryString(), GeneratePostBody()) :
                    await httpClient.GetAsync(GenerateQueryString());

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(withSelect != null ?
                JsonConvert.SerializeObject(withSelect(JsonConvert.DeserializeObject(content))) :
                content);
        }

        private string GenerateQueryString()
        {
            return asPost ?
                TRAKT_API_URL + path + (extended ? "/extended" : string.Empty) :
                parameters.GetType().GetProperty("query") != null ?
                TRAKT_API_URL + path + "?query=" + WebUtility.UrlEncode(parameters.query) :
                TRAKT_API_URL + path + "/" + parameters.username + (parameters.GetType().GetProperty("season") != null ? "/" + parameters.season : "") + (extended ? "/extended" : string.Empty);
        }

        private HttpContent GeneratePostBody()
        {
            var json = JsonConvert.SerializeObject(parameters);
            return new StringContent(json);
        }
    }

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

        public async Task AddShowToLibraryAsync(string username, string showTitle, int year)
        {
            await new TraktIO<dynamic>().
                ForPath("show/library").
                AsPost().
                WithParameters(new
            {
                username,
                title = showTitle,
                year
            }).
                Execute();
        }

        public Task<IList<TraktEpisode>> GetSeasonEpisodesAsync(string showId, int seasonNumber, bool extended = false)
        {
            return new TraktIO<IList<TraktEpisode>>().
                ForPath("show/season.json").
                WithParameters(new { username = showId, season = seasonNumber }).
                Execute();
        }

        public async Task<TraktUser> GetSettingsAsync()
        {
            var request = new RestRequest("users/settings");

            var response = await RestClient.Execute<TraktUser>(request);

            return response.Data;
        }

        public Task<TraktTVShow> GetShowAsync(string showId, bool extended = false)
        {
            return new TraktIO<TraktTVShow>().
                ForPath("show/summary.json").
                WithParameters(new { username = showId }).
                Extended(extended).
                Execute();
        }

        public async Task<IList<TraktTVShow>> GetWatchedEpisodesAsync(string username)
        {
            var request = new RestRequest("sync/watched/shows");

            var response = await RestClient.Execute<IList<TraktTVShow>>(request);

            return response.Data;
        }

        public Task<IList<TraktTVShow>> SearchTVShowAsync(string query)
        {
            return new TraktIO<IList<TraktTVShow>>().
                ForPath("search/shows").
                WithParameters(new { query }).
                Extended(false).
                Execute();

        }

        public async Task WatchEpisodesAsync(string username, string showTitle, int year, IList<dynamic> episodes)
        {
            await new TraktIO<dynamic>().
                ForPath("show/episode/seen").
                AsPost().
                WithParameters(new
            {
                username,
                title = showTitle,
                year,
                episodes
            }).
                Execute();
        }
    }
}

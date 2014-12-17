﻿using System;
using System.Threading.Tasks;
using Fresh.Windows.Core.Services.Interfaces;
using System.Net.Http;
using Newtonsoft.Json;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Fresh.Windows.Core.Models;
using System.Collections.Generic;

namespace Fresh.Windows.Core.Services
{
    internal class TraktIO<T>
    {
        private const string TRAKT_API_URL = @"http://api.trakt.tv/";

        private readonly string apiKey;
        private bool asPost;
        private bool extended;
        private string path;
        private Func<dynamic, dynamic> withSelect;

        private dynamic parameters;

        public TraktIO(string apiKey)
        {
            this.apiKey = apiKey;
        }

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
                TRAKT_API_URL + path + "/" + apiKey + (extended ? "/extended" : string.Empty) :
                TRAKT_API_URL + path + "/" + apiKey + "/" + parameters.username + (parameters.GetType().GetProperty("season") != null ? "/" + parameters.season : "") + (extended ? "/extended" : string.Empty);
        }

        private HttpContent GeneratePostBody()
        {
            var json = JsonConvert.SerializeObject(parameters);
            return new StringContent(json);
        }
    }

    public class TraktService : ITraktService
    {
        private readonly string apiKey;

        public TraktService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public Task<IList<TraktTVShow>> GetLibraryAsync(string username, bool extended = false)
        {
            return new TraktIO<IList<TraktTVShow>>(apiKey).
                ForPath("user/library/shows/collection.json").
                WithParameters(new { username }).
                Extended(extended).
                Execute();
        }

        public Task<IList<TraktEpisode>> GetSeasonEpisodesAsync(string showId, int seasonNumber, bool extended = false)
        {
            return new TraktIO<IList<TraktEpisode>>(apiKey).
                ForPath("show/season.json").
                WithParameters(new { username = showId, season = seasonNumber }).
                Execute();
        }

        public Task<dynamic> GetSettingsAsync(string username, string password)
        {
            return new TraktIO<dynamic>(apiKey).
                ForPath("account/settings").
                AsPost().
                WithParameters(new { username, password }).
                Select(content => content["profile"]).
                Execute();
        }

        public Task<TraktTVShow> GetShowAsync(string showId, bool extended = false)
        {
            return new TraktIO<TraktTVShow>(apiKey).
                ForPath("show/summary.json").
                WithParameters(new { username = showId }).
                Extended(extended).
                Execute();
        }


        public Task<IList<TraktTVShow>> GetWatchedEpisodesAsync(string username)
        {
            return new TraktIO<IList<TraktTVShow>>(apiKey).
                ForPath("user/library/shows/watched.json").
                WithParameters(new { username }).
                Extended(false).
                Execute();
        }


        public Task WatchEpisodeAsync(string username, string password, string showTitle, int year, int season, int episode)
        {
            var result = new TraktIO<dynamic>(apiKey).
                ForPath("show/episode/seen").
                AsPost().
                WithParameters(new
                {
                    username,
                    password,
                    title = showTitle,
                    year,
                    episodes = new[] 
                    {
                        new { season, episode, last_played = DateTime.UtcNow }
                    }
                }).
                Execute();

            return Task.FromResult<object>(null);
        }
    }
}

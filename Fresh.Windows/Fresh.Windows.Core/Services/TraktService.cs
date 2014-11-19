using System;
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

        public TraktIO<T> WithParameters(object parameters)
        {
            this.parameters = parameters;
            return this;
        }

        public TraktIO<T> Select(Func<dynamic, dynamic> withSelect)
        {
            this.withSelect = withSelect;
            return this;
        }

        public TraktIO<T> Extended()
        {
            this.extended = true;
            return this;
        }

        public async Task<T> Execute()
        {
            HttpResponseMessage response;
            using(var httpClient = new HttpClient())
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
                TRAKT_API_URL + path + "/" + apiKey + "/" + parameters.username + (extended ? "/extended" : string.Empty);
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

        public Task<IList<TVShow>> GetCollection(string username)
        {
            return new TraktIO<IList<TVShow>>(apiKey).
                ForPath("user/library/shows/collection.json").
                WithParameters(new { username }).
                Extended().
                Execute();
        }

        public Task<dynamic> GetSettings(string username, string password)
        {
            var passVector = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
            var digest = HashAlgorithmProvider.OpenAlgorithm("SHA1").HashData(passVector);

            return new TraktIO<dynamic>(apiKey).
                ForPath("account/settings").
                AsPost().
                WithParameters(new { username, password = CryptographicBuffer.EncodeToHexString(digest) }).
                Select(content => content["profile"]).
                Execute();
        }
    }
}

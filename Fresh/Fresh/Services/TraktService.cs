using System;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using System.Net.Http;

namespace Fresh
{
	public class TraktService : ITraktService
	{
		readonly string ClientId;
		readonly string ClientSecret;
		readonly Uri RedirectUri;

		readonly RestClient Client;

		public Uri AuthorizeUri
		{
			get
			{
				var request = new RestRequest("oauth/authorize")
					.AddQueryParameter("response_type", "code")
					.AddQueryParameter("redirect_uri", RedirectUri.AbsoluteUri)
					.AddQueryParameter("client_id", ClientId);

				return new RestClient("https://trakt.tv").BuildUri(request);
			}
		}

		class TraktAuthenticator : IAuthenticator
		{
			class OAuthToken
			{
				public string Access_Token { get; set; }

				public string Token_type { get; set; }

				public int Expires_In { get; set; }

				public string Refresh_Token { get; set; }

				public string Scope { get; set; }
			}

			OAuthToken token;

			readonly string ClientId;
			readonly string ClientSecret;
			readonly Uri RedirectUri;

			public TraktAuthenticator(string clientId, string clientSecret, Uri redirectUri)
			{
				RedirectUri = redirectUri;
				ClientSecret = clientSecret;
				ClientId = clientId;
			}

			public void Authenticate(IRestClient client, IRestRequest request)
			{
				if (token == null)
					throw new Exception("Invalid token. Make sure to call GetAccessTokenAsync prior to your first api call.");

				request.AddHeader("authorization", string.Format("{0} {1}", "Bearer"/*token.Token_type*/, token.Access_Token)).
				AddHeader("trakt-api-version", 2).
				AddHeader("trakt-api-key", ClientId);
			}

			public async Task GetAccessTokenAsync(IRestClient client, string authCode)
			{
				var request = new RestRequest("oauth/token", HttpMethod.Post)
					.AddJsonBody(new 
						{
							code = authCode,
							client_id = ClientId,
							client_secret = ClientSecret,
							redirect_uri = RedirectUri.AbsoluteUri,
							grant_type = "authorization_code"
						});

				var response = await client.Execute<OAuthToken>(request);
				token = response.Data;
			}
		}

		public TraktService(string clientId, string clientSecret, Uri redirectUri)
		{
			RedirectUri = redirectUri;
			ClientId = clientId;
			ClientSecret = clientSecret;

			Client = new RestClient("https://api-v2launch.trakt.tv");
		}

		public async Task LoginAsync(string authCode)
		{
			var authenticator = new TraktAuthenticator(ClientId, ClientSecret, RedirectUri);
			await authenticator.GetAccessTokenAsync(Client, authCode);

			Client.Authenticator = authenticator;
		}
	}
}


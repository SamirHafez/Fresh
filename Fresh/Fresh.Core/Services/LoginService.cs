using System.Threading.Tasks;
using Fresh.Core.Services.Interfaces;
using PCLStorage;
using Newtonsoft.Json;
using System;

namespace Fresh.Core.Services
{
	public class LoginService : ILoginService
	{
		OAuthToken token;

		public OAuthToken Token { get { return token; } }

		public readonly string ClientId;
		public readonly string ClientSecret;
		public readonly string TokenPath;
		public readonly Uri AuthorizeUri;
		public readonly Uri RedirectUri;
		public readonly Uri AccessUri;

		public LoginService(string tokenPath, string clientId, string clientSecret, Uri authorizeUri, Uri redirectUri, Uri accessUri)
		{
			ClientId = clientId;
			ClientSecret = clientSecret;
			TokenPath = tokenPath;
			AuthorizeUri = authorizeUri;
			RedirectUri = redirectUri;
			AccessUri = accessUri;
		}

		public async Task<bool> SilentLoginAsync()
		{
			var folder = await FileSystem.Current.LocalStorage.CreateFolderAsync("settings", CreationCollisionOption.OpenIfExists);

			if ((await folder.CheckExistsAsync(TokenPath)) == ExistenceCheckResult.NotFound)
				return false;

			var file = await folder.GetFileAsync(TokenPath);

			var fileText = await file.ReadAllTextAsync();

			token = JsonConvert.DeserializeObject<OAuthToken>(fileText);

			return true;
		}

		public async Task LauchLoginAsync(IOAuthBrokerService oauthBroker)
		{
			await oauthBroker.LaunchBrokerAsync(ClientId, ClientSecret, AuthorizeUri, RedirectUri, AccessUri);
		}

		public async Task SaveAsync(OAuthToken token)
		{
			var folder = await FileSystem.Current.LocalStorage.CreateFolderAsync("settings", CreationCollisionOption.OpenIfExists);

			var file = await folder.CreateFileAsync(TokenPath, CreationCollisionOption.ReplaceExisting);

			var json = JsonConvert.SerializeObject(token);
			await file.WriteAllTextAsync(json);
		}
	}
}

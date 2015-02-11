using System;
using Cirrious.MvvmCross.ViewModels;
using Fresh.Core.Services.Interfaces;
using System.Threading.Tasks;

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
			string accessToken = await oauthService.GetAccessTokenAsync(ClientId, ClientSecret, AuthorizeUri, RedirectUri, AccessUri);
		}
	}
}

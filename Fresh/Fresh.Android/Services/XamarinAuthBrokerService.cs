using System;
using System.Threading.Tasks;
using Fresh.Core.Services.Interfaces;
using Xamarin.Auth;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;

namespace Fresh.Android.Services
{
	public class XamarinAuthBrokerService : IOAuthBrokerService
	{
		public async Task<string> GetAccessTokenAsync(string clientId, string clientSecret, Uri authorizeUri, Uri redirectUri, Uri accessUri)
		{
			var auth = new OAuth2Authenticator(
				clientId: clientId,
				clientSecret: clientSecret,
				scope: string.Empty,
				authorizeUrl: authorizeUri,
				redirectUrl: redirectUri,
				accessTokenUrl: accessUri);

			var tcs = new TaskCompletionSource<Account>();

			auth.Completed += delegate (object sender, AuthenticatorCompletedEventArgs e)
			{
				if (e.IsAuthenticated)
					tcs.SetResult(e.Account);
				else
					tcs.SetCanceled();
			};

			var activity = Mvx.GetSingleton<IMvxAndroidCurrentTopActivity>().Activity;

			activity.StartActivity(auth.GetUI(activity));

			Account account = await tcs.Task;

			return account.Username;
		}
	}
}
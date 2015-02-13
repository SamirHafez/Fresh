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
        public Task LaunchBrokerAsync(string clientId, string clientSecret, Uri authorizeUri, Uri redirectUri, Uri accessUri)
        {
            var auth = new OAuth2Authenticator(
                clientId: clientId,
                clientSecret: clientSecret,
                scope: string.Empty,
                authorizeUrl: authorizeUri,
                redirectUrl: redirectUri,
                accessTokenUrl: accessUri
                );

            var tcs = new TaskCompletionSource<Account>();

            auth.Completed += (sender, e) => tcs.SetResult(e.Account);

            var activity = Mvx.GetSingleton<IMvxAndroidCurrentTopActivity>().Activity;

            activity.StartActivity(auth.GetUI(activity));

            return tcs.Task;
        }
    }
}
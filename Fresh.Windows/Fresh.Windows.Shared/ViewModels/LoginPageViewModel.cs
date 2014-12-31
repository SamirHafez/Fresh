using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using Windows.UI.Popups;
using Windows.Security.Authentication.Web;
using Fresh.Windows.Core.Services;
using Windows.UI.Core;

namespace Fresh.Windows.ViewModels
{
    public class LoginPageViewModel : ViewModel, ILoginPageViewModel
    {
        private readonly ILoginService loginService;
        private readonly INavigationService navigationService;

        public DelegateCommand LoginCommand { get { return new DelegateCommand(Login); } }

        public LoginPageViewModel(ILoginService loginService, INavigationService navigationService)
        {
            this.loginService = loginService;
            this.navigationService = navigationService;
        }

        private async void Login()
        {
            var callbackUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();
            var uri = new Uri(string.Format("https://api.trakt.tv/oauth/authorize?response_type=code&client_id={0}&redirect_uri={1}",
                App.TRAKT_CLIENT_ID, callbackUri.AbsoluteUri));

            var response = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, uri, callbackUri);

            if (response.ResponseStatus != WebAuthenticationStatus.Success)
                return;

            var code = response.ResponseData.Substring(response.ResponseData.LastIndexOf("code=") + "code=".Length);

            Exception exception = null;
            var oauthRequest = new OAuthRequest
            {
                code = code,
                redirect_uri = callbackUri.AbsoluteUri,
                grant_type = "authorization_code"
            };

            try
            {
                await loginService.LoginAsync(oauthRequest);
                string username = string.Empty;
                navigationService.Navigate(App.Experience.Main.ToString(), username);
            }
            catch (Exception e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error").ShowAsync();
                return;
            }
        }
    }
}

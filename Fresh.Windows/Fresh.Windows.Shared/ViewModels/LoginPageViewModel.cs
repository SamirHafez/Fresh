using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using Windows.UI.Popups;
using Windows.Security.Authentication.Web;
using Fresh.Windows.Core.Services;

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
            var clientId = "cad18e7e5ba53d5cd88652204a774d1e5c3bd69a20ca04899102d616df58f71f";
            var uri = new Uri(string.Format("https://api.trakt.tv/oauth/authorize?response_type=code&client_id={0}&redirect_uri={1}", clientId, WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri));

            var response = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, uri);

            if (response.ResponseStatus != WebAuthenticationStatus.Success)
            {
                await new MessageDialog("Login failed.", "Error").ShowAsync();
                return;
            }

            var code = response.ResponseData.Substring(response.ResponseData.LastIndexOf("code=") + "code=".Length);

            Exception exception = null;
            var oauthRequest = new OAuthRequest
            {
                code = code,
                client_id = "cad18e7e5ba53d5cd88652204a774d1e5c3bd69a20ca04899102d616df58f71f",
                client_secret = "9f2e811e93c05e29d61812146fa2e12dbf6cbf9caff639b1de23465b2e2817cb",
                redirect_uri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri,
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

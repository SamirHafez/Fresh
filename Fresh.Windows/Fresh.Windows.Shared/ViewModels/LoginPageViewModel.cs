using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using Windows.UI.Popups;

namespace Fresh.Windows.ViewModels
{
    public class LoginPageViewModel : ViewModel, ILoginPageViewModel
    {
        private readonly ILoginService loginService;
        private readonly INavigationService navigationService;

        public LoginPageViewModel(ILoginService loginService, INavigationService navigationService)
        {
            this.loginService = loginService;
            this.navigationService = navigationService;
        }

        string username = default(string);
        public string Username { get { return username; } set { SetProperty(ref username, value); } }

        string password = default(string);
        public string Password { get { return password; } set { SetProperty(ref password, value); } }

        public DelegateCommand LoginCommand
        {
            get
            {
                return new DelegateCommand(Login);
            }
        }

        private async void Login()
        {
            try
            {
                await loginService.LoginAsync(Username, Password);
                navigationService.Navigate(App.Experience.Main.ToString(), Username);
            }
            catch (Exception exception)
            {
                await new MessageDialog(exception.Message, "Error").ShowAsync();
            }
        }
    }
}

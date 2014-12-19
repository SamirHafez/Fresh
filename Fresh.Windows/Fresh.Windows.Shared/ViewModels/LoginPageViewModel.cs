using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using Windows.UI.Popups;
using System.Collections.Generic;

namespace Fresh.Windows.ViewModels
{
    public class LoginPageViewModel : ViewModel, ILoginPageViewModel
    {
        private readonly ILoginService loginService;
        private readonly INavigationService navigationService;

        private readonly DelegateCommand loginCommand;

        public LoginPageViewModel(ILoginService loginService, INavigationService navigationService)
        {
            this.loginService = loginService;
            this.navigationService = navigationService;

            this.loginCommand = new DelegateCommand(Login,
                    () => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password));
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);

            navigationService.ClearHistory();
        }

        string username = default(string);
        public string Username { get { return username; } set { SetProperty(ref username, value); loginCommand.RaiseCanExecuteChanged(); } }

        string password = default(string);
        public string Password { get { return password; } set { SetProperty(ref password, value); loginCommand.RaiseCanExecuteChanged(); } }

        bool working = default(bool);
        public bool Working { get { return working; } set { SetProperty(ref working, value); } }

        public DelegateCommand LoginCommand { get { return loginCommand; } }

        private async void Login()
        {
            Working = true;
            Exception exception = null;
            try
            {
                await loginService.LoginAsync(Username, Password);
                navigationService.Navigate(App.Experience.Main.ToString(), Username);
            }
            catch (Exception ex)
            {
                exception = ex;
                Working = false;
            }

            if (exception != null)
                await new MessageDialog(exception.Message, "Error").ShowAsync();
        }
    }
}

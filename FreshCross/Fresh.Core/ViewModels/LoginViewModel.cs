using System;
using Cirrious.MvvmCross.ViewModels;
using Fresh.Core.Services.Interfaces;
using Fresh.Core.Services;
using System.Threading.Tasks;

namespace Fresh.Core.ViewModels
{
	public class LoginViewModel : MvxViewModel
	{
		IOAuthBrokerService oauthService;
		ITraktService traktService;
		ILoginService loginService;

		string authCode;
		public string AuthCode { get { return authCode; } set { authCode = value; RaisePropertyChanged(() => AuthCode); doneCommand.RaiseCanExecuteChanged(); } }

		IMvxCommand doneCommand;
		public IMvxCommand DoneCommand
		{
			get { return doneCommand ?? (doneCommand = new MvxCommand(async () => await CodeObtainedAsync(), () => !string.IsNullOrWhiteSpace(AuthCode))); }
		}

		public LoginViewModel(IOAuthBrokerService oauthService, ITraktService traktService, ILoginService loginService)
		{
			this.oauthService = oauthService;
			this.traktService = traktService;
			this.loginService = loginService;
		}

		protected override async void InitFromBundle(IMvxBundle parameters)
		{
			base.InitFromBundle(parameters);

            //await loginService.LauchLoginAsync(oauthService);
		}

		private async Task CodeObtainedAsync()
		{
			OAuthToken token = await traktService.SetAuthCodeAsync(AuthCode);
			await loginService.SaveAsync(token);
			ShowViewModel<MainViewModel>();
		}
	}
}
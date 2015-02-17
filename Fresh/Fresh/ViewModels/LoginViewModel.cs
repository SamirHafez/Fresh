using System;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Fresh
{
	public class LoginViewModel : ViewModelBase
	{
		public Uri AuthorizeUri
		{
			get
			{
				return traktService.AuthorizeUri;
			}
		}

		string code;

		public string Code
		{
			get { return code; }
			set { SetProperty(ref code, value); }
		}

		public ICommand DoneCommand
		{
			get
			{
				return new Command<string>(
					async code => await HandleAuthCodeAsync(code), 
					code => !string.IsNullOrWhiteSpace(code)); 
			}
		}

		readonly ITraktService traktService;
		readonly INavigator navigator;

		public LoginViewModel(ITraktService traktService, INavigator navigator)
		{
			this.navigator = navigator;
			this.traktService = traktService;
		}

		async Task HandleAuthCodeAsync(string authCode)
		{
			try
			{
				await traktService.LoginAsync(authCode);

				await navigator.PushAsync<MainViewModel>();
			}
			catch (Exception e)
			{
				//TODO - Handle error (maybe display to the user?)
			}
		}
	}
}

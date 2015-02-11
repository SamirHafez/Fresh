using Cirrious.MvvmCross.ViewModels;

namespace Fresh.Core.ViewModels
{
	public class LoginViewModel : MvxViewModel
	{
		string hello = default(string);
		public string Hello
		{
			get { return hello; }
			set { hello = value; RaisePropertyChanged(() => Hello); }
		}
	}
}

using System;

namespace Fresh
{
	public class LoginViewModel : ViewModelBase
	{
		const string ClientId = "ad18e7e5ba53d5cd88652204a774d1e5c3bd69a20ca04899102d616df58f71f";
		readonly Uri RedirectUri = new Uri("urn:ietf:wg:oauth:2.0:oob");

		public Uri AuthorizeUri
		{
			get
			{
				return new Uri(
					string.Format("https://trakt.tv/oauth/authorize?response_type=code&client_id={0}&redirect_uri={1}",
						ClientId, 
						RedirectUri.AbsolutePath));
			}
		}
	}
}

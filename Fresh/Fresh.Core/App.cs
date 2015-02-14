using System;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore;
using Fresh.Core.Services;
using Fresh.Core.Services.Interfaces;
using Cirrious.MvvmCross.Plugins.File;

namespace Fresh.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
		const string ClientId = "cad18e7e5ba53d5cd88652204a774d1e5c3bd69a20ca04899102d616df58f71f";
		const string ClientSecret = "9f2e811e93c05e29d61812146fa2e12dbf6cbf9caff639b1de23465b2e2817cb";
		static Uri AuthorizeUri = new Uri("https://trakt.tv/oauth/authorize");
		static Uri RedirectUri = new Uri("urn:ietf:wg:oauth:2.0:oob");
		static Uri AccessUri = new Uri("https://trakt.tv/oauth/token");

		public override async void Initialize()
        {
			//CreatableTypes()
			//    .EndingWith("Service")
			//    .AsInterfaces()
			//    .RegisterAsLazySingleton();

            var fileStore = Mvx.Resolve<IMvxFileStore>(); 
			var loginService = new LoginService("perm.token", ClientId, ClientSecret, AuthorizeUri, RedirectUri, AccessUri, fileStore);
			Mvx.RegisterSingleton<ILoginService>(loginService);

			var hasLogin = loginService.SilentLoginAsync().Result;

			var traktService = new TraktService(ClientId, ClientSecret, RedirectUri, loginService.Token);
			Mvx.RegisterSingleton<ITraktService>(traktService);

            if (hasLogin)
                RegisterAppStart<ViewModels.MainViewModel>();
            else
                RegisterAppStart<ViewModels.LoginViewModel>();

        }
	}
}
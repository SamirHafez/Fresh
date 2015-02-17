using System;
using Autofac;

namespace Fresh
{
	public class FreshModule : Module
	{
		const string TRAKT_CLIENT_ID = "cad18e7e5ba53d5cd88652204a774d1e5c3bd69a20ca04899102d616df58f71f";
		const string TRAKT_CLIENT_SECRET = "9f2e811e93c05e29d61812146fa2e12dbf6cbf9caff639b1de23465b2e2817cb";
		const string TRAKT_REDIRECT = "urn:ietf:wg:oauth:2.0:oob";

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<LoginViewModel>()
				.SingleInstance();

			builder.RegisterType<LoginView>()
				.SingleInstance();

			builder.RegisterType<MainViewModel>()
				.SingleInstance();

			builder.RegisterType<MainView>()
				.SingleInstance();

			builder.Register(context => new TraktService(TRAKT_CLIENT_ID, TRAKT_CLIENT_SECRET, new Uri(TRAKT_REDIRECT)))
				.As<ITraktService>()
				.SingleInstance();
		}
	}
}


using System;
using Autofac;

namespace Fresh
{
	public class Bootstrapper : AutofacBootstrapper
	{
		private readonly App _application;

		public Bootstrapper(App application)
		{
			_application = application;           
		}

		protected override void ConfigureContainer(ContainerBuilder builder)
		{
			base.ConfigureContainer(builder);
			builder.RegisterModule<FreshModule>();
		}

		protected override void RegisterViews(IViewFactory viewFactory)
		{
			viewFactory.Register<LoginViewModel, LoginView>();
		}

		protected override void ConfigureApplication(IContainer container)
		{
			// set main page
			var viewFactory = container.Resolve<IViewFactory>();
			var loginPage = viewFactory.Resolve<LoginViewModel>();

			_application.MainPage = loginPage;
		}

	}
}


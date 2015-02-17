using System;

using Xamarin.Forms;
using Autofac;

namespace Fresh
{
	public class App : Application
	{
		public App()
		{
			// The root page of your application
			var bootstrapper = new Bootstrapper(this);
			bootstrapper.Run();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}


﻿using System;
using Autofac;
using Xamarin.Forms;

namespace Fresh
{
	public class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ViewFactory>()
				.As<IViewFactory>()
				.SingleInstance();

			builder.RegisterType<Navigator>()
				.As<INavigator>()
				.SingleInstance();

			builder.Register<INavigation>(context =>
				Application.Current.MainPage.Navigation)
				.SingleInstance();
		}
	}
}


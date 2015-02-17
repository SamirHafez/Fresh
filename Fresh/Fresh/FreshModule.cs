using System;
using Autofac;

namespace Fresh
{
	public class FreshModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<LoginViewModel>()
				.SingleInstance();

			builder.RegisterType<LoginView>()
				.SingleInstance();
		}
	}
}


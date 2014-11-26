﻿using Fresh.Windows.Core.Services;
using Fresh.Windows.Core.Services.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Fresh.Windows
{
    public sealed partial class App : MvvmAppBase
    {
        public enum Experience { Main, Login, TVShow, Season };

        private readonly UnityContainer container = new UnityContainer();

        private readonly string TRAKT_APIKEY = "68d5502c616356a5a844d284c546032d";

        public App()
        {
            this.InitializeComponent();
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            container.RegisterInstance<INavigationService>(NavigationService);
            container.RegisterType<IConfigurationService, FreshConfigurationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILoginService, LoginService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IStorageService, PCLStorageService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICrawlerService, FreeTVCrawler>();
            container.RegisterInstance<ITraktService>(new TraktService(TRAKT_APIKEY), new ContainerControlledLifetimeManager());

            return Task.FromResult<object>(null);
        }

        protected override object Resolve(Type type)
        {
            return container.Resolve(type);
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            ILoginService service = container.Resolve<ILoginService>();

            if (await service.SilentLoginAsync())
                NavigationService.Navigate(Experience.Main.ToString(), null);
            else
                NavigationService.Navigate(Experience.Login.ToString(), null);
        }
    }
}
﻿using Fresh.Windows.Core.Services;
using Fresh.Windows.Core.Services.Interfaces;
using Fresh.Windows.Helpers;
using Fresh.Windows.Shared.Services;
using Fresh.Windows.Shared.Services.Interfaces;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using SQLite.Net;
using SQLite.Net.Interop;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace Fresh.Windows
{
    public sealed partial class App : MvvmAppBase
    {
        public enum Experience { Main, Login, TVShow, Season, Episode, SearchResults };

        private readonly UnityContainer container = new UnityContainer();

        private static readonly string APPLICATION_PATH = ApplicationData.Current.LocalFolder.Path;
        public const string TRAKT_CLIENT_ID = "cad18e7e5ba53d5cd88652204a774d1e5c3bd69a20ca04899102d616df58f71f";
        private const string TRAKT_CLIENT_SECRET = "9f2e811e93c05e29d61812146fa2e12dbf6cbf9caff639b1de23465b2e2817cb";

        public App()
        {
            this.InitializeComponent();
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            container.RegisterInstance<INavigationService>(NavigationService);
            container.RegisterType<ILoginService, LoginService>();
            container.RegisterInstance<SQLiteConnectionString>(new SQLiteConnectionString(APPLICATION_PATH + @"\fresh.db", storeDateTimeAsTicks: false), new ContainerControlledLifetimeManager());
            container.RegisterType<ISQLitePlatform, SQLitePlatformWP81>(new ContainerControlledLifetimeManager());
            container.RegisterType<IStorageService, SQLiteService>();
            container.RegisterType<ICrawlerService, LetMeWatchThisCrawlerService>();
            container.RegisterInstance<ITraktService>(new TraktService(TRAKT_CLIENT_ID, TRAKT_CLIENT_SECRET), new ContainerControlledLifetimeManager());

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
using Fresh.Windows.Core.Services;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using System;
using Fresh.Windows.Core.Services.Interfaces;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Fresh.Windows.Shared.Configuration;
using Fresh.Windows.Shared.Services.Interfaces;
using Fresh.Windows.Shared.Services;
using SQLite.Net.Interop;
using Fresh.Windows.Helpers;
using Windows.Storage;
using SQLite.Net;

namespace Fresh.Windows
{
    public sealed partial class App : MvvmAppBase
    {
        public enum Experience { Main, Login, TVShow, Season, Episode };

        private readonly UnityContainer container = new UnityContainer();

        private const string TRAKT_APIKEY = "68d5502c616356a5a844d284c546032d";

        private static readonly string APPLICATION_PATH = ApplicationData.Current.LocalFolder.Path;

        public App()
        {
            this.InitializeComponent();
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            container.RegisterInstance<INavigationService>(NavigationService);
            container.RegisterType<ISession, FreshSession>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILoginService, LoginService>();
            container.RegisterInstance<SQLiteConnectionString>(new SQLiteConnectionString(APPLICATION_PATH + @"\fresh.db", storeDateTimeAsTicks: false), new ContainerControlledLifetimeManager());
            container.RegisterType<ISQLitePlatform, SQLitePlatformW81>(new ContainerControlledLifetimeManager());
            container.RegisterType<IStorageService, SQLiteService>();
            container.RegisterType<ICrawlerService, LetMeWatchThisCrawlerService>();
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

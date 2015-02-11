using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Fresh.Android.Services;
using Fresh.Core.Services.Interfaces;

namespace Fresh.Android
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

		protected override void InitializeFirstChance()
		{
			Mvx.RegisterSingleton<IOAuthBrokerService>(new XamarinAuthBrokerService());
			base.InitializeFirstChance();
		}
	}
}
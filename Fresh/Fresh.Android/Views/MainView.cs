
using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace Fresh.Android.Views
{
	[Activity(Label = "Main")]
	public class MainView : MvxActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.MainView);
		}
	}
}
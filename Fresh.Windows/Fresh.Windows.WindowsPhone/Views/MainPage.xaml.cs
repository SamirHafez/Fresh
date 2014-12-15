using Fresh.Windows.Shared.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Fresh.Windows.Views
{
    public sealed partial class MainPage : PageBase
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            VisualStateManager.GoToState(this, "Loading", false);
        }
    }
}

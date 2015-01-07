using Fresh.Windows.Shared.Controls;
using Windows.UI.Xaml;

namespace Fresh.Windows.Views
{
    public sealed partial class EpisodePage : PageBase
    {
        public EpisodePage()
        {
            this.InitializeComponent();

            this.playButton.Click += (sender, args) => VisualStateManager.GoToState(this, "Playing", useTransitions: true);
            this.moreButton.Click += (sender, args) => VisualStateManager.GoToState(this, "Idle", useTransitions: true);
        }
    }
}

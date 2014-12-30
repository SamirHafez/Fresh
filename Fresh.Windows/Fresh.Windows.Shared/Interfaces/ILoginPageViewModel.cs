using Microsoft.Practices.Prism.Commands;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface ILoginPageViewModel
    {
        DelegateCommand LoginCommand { get; }
    }
}

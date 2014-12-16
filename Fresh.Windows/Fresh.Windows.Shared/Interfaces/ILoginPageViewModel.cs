using Microsoft.Practices.Prism.Commands;
using System;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface ILoginPageViewModel
    {
        string Username { get; set; }
        string Password { get; set; }
        bool Working { get; set; }

        DelegateCommand LoginCommand { get; }
    }
}

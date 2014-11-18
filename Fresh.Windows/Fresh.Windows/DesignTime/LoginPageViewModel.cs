using System;
using Fresh.Windows.Interfaces;

namespace Fresh.Windows.DesignTime
{
    public class LoginPageViewModel : ILoginPageViewModel
    {
        public LoginPageViewModel()
        {
            Username = "DesignUser";
            Password = "DesignPass";
            LoginRunning = false;
        }

        public string Username { get; set; }
        public string Password { get; set; } 
        public bool LoginRunning { get; set; }
    }
}

using Fresh.Windows.Shared.Interfaces;

namespace Fresh.Windows.Shared.DesignTime
{
    public class LoginPageViewModel : ILoginPageViewModel
    {
        public LoginPageViewModel()
        {
            Username = "DesignUser";
            Password = "DesignPass";
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public bool Working { get; set; }
    }
}

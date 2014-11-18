namespace Fresh.Windows.Interfaces
{
    public interface ILoginPageViewModel
    {
        string Username { get; set; }
        string Password { get; set; }

        bool LoginRunning { get;set; }
    }
}

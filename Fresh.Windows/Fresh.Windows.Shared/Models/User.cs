using SQLite.Net.Attributes;

namespace Fresh.Windows.Shared.Models
{
    public class User
    {
        [PrimaryKey]
        public string Username { get; set; }

        public string AccessToken { get; set; }
        public string Refresh_Token { get; set; }
    }
}

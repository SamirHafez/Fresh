using SQLite.Net.Attributes;

namespace Fresh.Windows.Shared.Models
{
    public class User
    {
        [PrimaryKey]
        public string Username { get; set; }
    }
}

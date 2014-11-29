using SQLite;

namespace Fresh.Windows.Core.Models
{
    public class User
    {
        [PrimaryKey]
        public string Username { get; set; }
    }
}

using SQLite.Net.Attributes;
using System;

namespace Fresh.Windows.Shared.Models
{
    public class User
    {
        [PrimaryKey]
        public string Username { get; set; }

        public string AccessToken { get; set; }
        public string Refresh_Token { get; set; }

        public DateTime? ActivityUpdated { get; set; }
    }
}

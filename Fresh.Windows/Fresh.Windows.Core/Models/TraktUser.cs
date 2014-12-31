using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Models
{
    public class TraktAvatar
    {
        public string Full { get; set; }
    }

    public class TraktUserImages
    {
        public TraktAvatar Avatar { get; set; }
    }

    public class TraktUserInfo
    {
        public string Username { get; set; }
        public bool Private { get; set; }
        public string Name { get; set; }
        public bool Vip { get; set; }
        public string Joined_At { get; set; }
        public string Location { get; set; }
        public string About { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public TraktUserImages Images { get; set; }
    }

    public class TraktAccount
    {
        public string Timezone { get; set; }
        public object Cover_Image { get; set; }
    }

    public class TraktConnections
    {
        public bool Facebook { get; set; }
        public bool Twitter { get; set; }
        public bool Google { get; set; }
        public bool Tumblr { get; set; }
    }

    public class TraktSharingText
    {
        public string Watching { get; set; }
        public string Watched { get; set; }
    }

    public class TraktUser
    {
        public TraktUserInfo User { get; set; }
        public TraktAccount Account { get; set; }
        public TraktConnections Connections { get; set; }
        public TraktSharingText Sharing_Text { get; set; }
    }
}

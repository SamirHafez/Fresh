using Fresh.Windows.Core.Models;

namespace Fresh.Windows.Core.Configuration
{
    public class FreshSession : ISession
    {
        public User User { get; set; }
    }
}

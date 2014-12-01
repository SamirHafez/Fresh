using Fresh.Windows.Shared.Models;

namespace Fresh.Windows.Shared.Configuration
{
    public class FreshSession : ISession
    {
        public User User { get; set; }
    }
}

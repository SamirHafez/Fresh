using Fresh.Windows.Shared.Models;

namespace Fresh.Windows.Shared.Configuration
{
    public interface ISession
    {
        User User { get; set; }
    }
}

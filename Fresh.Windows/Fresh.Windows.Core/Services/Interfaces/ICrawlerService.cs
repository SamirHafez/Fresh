using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services.Interfaces
{
    public interface ICrawlerService
    {
        Task<string> GetLink(string tvShow, int season, int episode);
    }
}

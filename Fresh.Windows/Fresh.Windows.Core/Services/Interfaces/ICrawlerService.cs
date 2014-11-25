using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services.Interfaces
{
    public interface ICrawlerService
    {
        Task<IList<string>> GetLinks(string tvShow, int season, int episode);
    }
}

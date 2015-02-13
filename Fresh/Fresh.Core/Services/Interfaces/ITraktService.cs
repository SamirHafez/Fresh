using Fresh.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fresh.Core.Services.Interfaces
{
	public interface ITraktService
	{
		Task<OAuthToken> SetAuthCodeAsync(string code);
		Task<IList<TVShow>> GetPopularShowsAsync(TraktExtendEnum extended = TraktExtendEnum.MIN, int page = 1, int limit = 10);
    }
}

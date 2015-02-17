using System;
using System.Threading.Tasks;

namespace Fresh
{
	public interface ITraktService
	{
		Uri AuthorizeUri { get; }

		Task LoginAsync(string authCode);
	}
}


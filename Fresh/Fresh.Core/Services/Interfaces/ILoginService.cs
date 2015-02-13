using System.Threading.Tasks;

namespace Fresh.Core.Services.Interfaces
{
	public interface ILoginService
	{
		OAuthToken Token { get; }
		Task<bool> SilentLoginAsync();

		Task LauchLoginAsync(IOAuthBrokerService brokerService);
		Task SaveAsync(OAuthToken token);
	}
}

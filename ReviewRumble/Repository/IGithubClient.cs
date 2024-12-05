using Refit;
using ReviewRumble.Models;

namespace ReviewRumble.Repository
{
	public interface IGithubClient
	{

		[Post("/login/oauth/access_token")]
		Task<AccessTokenResponse> GenerateAccessTokenAsync([Query] AccessTokenRequestParams accessTokenRequestParams);
	}
}
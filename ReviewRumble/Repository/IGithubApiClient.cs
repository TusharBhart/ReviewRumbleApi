using Refit;
using ReviewRumble.Models;

namespace ReviewRumble.Repository
{
	public interface IGithubApiClient
	{
		[Get("/user")]
		Task<GitUser> GetUserAsync([Authorize("Bearer")] string accessToken);

		[Get("/user")]
		Task<GitUser> GetUserAsync();
	}
}
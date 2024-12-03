using Refit;
namespace ReviewRumble.Repository
{
	public interface IGithubApiClient
	{
		[Get("/repos/{owner}/{repo}/pulls")]
		Task<List<string>> GetRaisedPullRequests(string owner, string repo);

		[Get("/user")]
		Task<List<string>> GetUser();

		[Post("/login/oauth/access_token")]
		Task<object> GenerateAccessToken([Query] string client_id, [Query] string client_secret, [Query] string code);
	}
}
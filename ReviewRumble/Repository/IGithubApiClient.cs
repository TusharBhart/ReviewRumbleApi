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
		Task<object> GenerateAccessToken([Query] AccessTokenRequestParams accessTokenRequest);
	}

    public class AccessTokenRequestParams
    {
		[AliasAs("code")]
        public string Code { get; set; }

        [AliasAs("client_id")]
		public string ClientId { get; set; }

        [AliasAs("client_secret")]
		public string ClientSecret { get; set; }
    }
}
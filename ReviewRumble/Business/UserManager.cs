using Microsoft.Extensions.Caching.Memory;
using ReviewRumble.Repository;
using ReviewRumble.utils;

namespace ReviewRumble.Business
{
	public class UserManager : IUserManager
	{
		private IGithubApiClient GithubApiClient { get; set; }
		private  IMemoryCache MemoryCache { get; set; }

		public UserManager(IGithubApiClient githubApiClient , IMemoryCache memoryCache)
		{
			GithubApiClient = githubApiClient;
			MemoryCache = memoryCache;
		}
		public async Task<string> GetAccessToken(string clientId, string clientSecret, string code)
		{
			var accessTokenResponse = await GithubApiClient.GenerateAccessToken(clientId, clientSecret, code);
			if (accessTokenResponse.GetType().GetProperty("access_token") == null)
				throw new UnauthorizedAccessException();
			MemoryCache.Set("username", accessTokenResponse, DateTimeOffset.UtcNow.AddHours(1));
			return JwtManager.GenerateToken("username");
		}

		public async Task<object> GetUser()
		{
			var user = await GithubApiClient.GetUser();
			return user;
		}
	}
}
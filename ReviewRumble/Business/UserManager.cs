using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ReviewRumble.Models;
using ReviewRumble.Repository;
using ReviewRumble.utils;

namespace ReviewRumble.Business
{
	public class UserManager : IUserManager
	{
		private IGithubApiClient GithubApiClient { get; set; }
		private  IMemoryCache MemoryCache { get; set; }
		private GithubClientSettings GithubClientSettings { get; set; }

		public UserManager(IGithubApiClient githubApiClient , IMemoryCache memoryCache, IOptions<GithubClientSettings> settings)
		{
			GithubApiClient = githubApiClient;
			MemoryCache = memoryCache;
            GithubClientSettings = settings.Value;
        }

		public async Task<string> GetAccessToken(string code)
        {
            var accessTokenRequest = new AccessTokenRequestParams()
            {
                ClientId = GithubClientSettings.ClientId,
                ClientSecret = GithubClientSettings.ClientSecret,
                Code = code
            };
			var accessTokenResponse = await GithubApiClient.GenerateAccessToken(accessTokenRequest);

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
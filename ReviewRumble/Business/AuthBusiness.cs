using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ReviewRumble.Models;
using ReviewRumble.Repository;
using ReviewRumble.utils;

namespace ReviewRumble.Business
{
	public class AuthBusiness : IAuthBusiness
	{
		private IGithubApiClient GithubApiClient { get; set; }
		private IMemoryCache MemoryCache { get; set; }
		private IDataRepository DataRepository { get; set; }
		private IGithubClient GithubClient { get; set; }
		private GithubClientSettings GithubClientSettings { get; set; }

		public AuthBusiness(IGithubApiClient githubApiClient, IMemoryCache memoryCache,
			IOptions<GithubClientSettings> settings, IDataRepository dataRepository, IGithubClient githubClient)
		{
			GithubApiClient = githubApiClient;
			GithubClient = githubClient;
			MemoryCache = memoryCache;
			GithubClientSettings = settings.Value;
			DataRepository = dataRepository;
		}

		public async Task<string> GetAccessTokenAsync(string code)
		{
			var accessTokenRequest = new AccessTokenRequestParams()
			{
				ClientId = GithubClientSettings.ClientId,
				ClientSecret = GithubClientSettings.ClientSecret,
				Code = code
			};
			
			var accessTokenResponse = await GithubClient.GenerateAccessTokenAsync(accessTokenRequest);

			if (accessTokenResponse.AccessToken != null)
			{
				return await OnSuccessFullyGettingAccessTokenAsync(accessTokenResponse);
			}

			throw new Exception("Not Authorized");
		}

		private async Task<string> OnSuccessFullyGettingAccessTokenAsync(AccessTokenResponse accessTokenResponse)
		{
			var user = await GithubApiClient.GetUserAsync(accessTokenResponse.AccessToken);
			await AddIfNewUserAsync(user);
			MemoryCache.Set(user.Id, accessTokenResponse, DateTimeOffset.UtcNow.AddMinutes(28800));
			return await JwtManager.GenerateTokenAsync(user);
		}

		private async Task AddIfNewUserAsync(GitUser gitUser)
		{
			//if (await DataRepository.GetUserByUserNameAsync(gitUser.Username) == null)
			//{
			//	var user = new User()
			//	{
			//		Username = gitUser.Login,
			//		Id = gitUser.Id
			//	};
			//	await DataRepository.AddReviewerAsync(user);
			//}
		}
	}
}

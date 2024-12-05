using Refit;

namespace ReviewRumble.Models
{
	public class AccessTokenResponse
	{
		[AliasAs("access_token")]
		public string? AccessToken { get; set; }

		[AliasAs("refresh_token")]
		public string? RefreshToken { get; set; }
	}
}

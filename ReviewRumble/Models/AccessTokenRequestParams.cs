using Refit;

namespace ReviewRumble.Models
{
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

namespace ReviewRumble.Models
{
	public class JwtOption
	{
		public const string SectionName = "Jwt";
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public string Secret { get; set; }
		public int? ExpiryInMinutes { get; set; } = 60;

	}
}

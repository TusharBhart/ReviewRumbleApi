namespace ReviewRumble.Models
{
    public class GithubClientSettings
    {
        public const string GithubApiClientSettings = "GithubApiClientSettings";

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string BaseUrl { get; set; }
        public string Timeout { get; set; }
    }
}

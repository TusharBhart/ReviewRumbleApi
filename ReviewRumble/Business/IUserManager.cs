namespace ReviewRumble.Business
{
	public interface IUserManager
	{
		public Task<string> GetAccessToken(string clientId, string clientSecret, string code);
		public Task<object> GetUser();
	}
}
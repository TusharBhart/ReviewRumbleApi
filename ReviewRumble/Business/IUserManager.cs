namespace ReviewRumble.Business
{
	public interface IUserManager
	{
		public Task<string> GetAccessToken(string code);
		public Task<object> GetUser();
	}
}
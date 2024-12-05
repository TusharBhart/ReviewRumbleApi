namespace ReviewRumble.Business
{
	public interface IAuthBusiness
	{
		public Task<string> GetAccessTokenAsync(string code);
	}
}

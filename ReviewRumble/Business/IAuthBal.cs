namespace ReviewRumble.Business
{
	public interface IAuthBal
	{
		public Task<string> GetAccessTokenAsync(string code);
	}
}

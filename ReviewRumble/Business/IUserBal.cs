using ReviewRumble.Models;

namespace ReviewRumble.Business;
public interface IUserBal
{
	public Task<UserInfo> GetUser(string author);
}

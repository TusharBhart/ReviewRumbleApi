using ReviewRumble.Models;

namespace ReviewRumble.Business;
public interface IUserBal
{
	public Task<UserInfo> GetUserAsync(string author);
	public Task<List<PullRequestViewModel>> GetAssignedPullRequestsAsync(int id);
	public Task<List<PullRequestViewModel>> GetMyPullRequestsAsync(int id);
	public Task UpdateStatusAsync(int id, ReviewerStatusEnum status);
	public Task<List<UserInfo>> GetLeaderboard();
}

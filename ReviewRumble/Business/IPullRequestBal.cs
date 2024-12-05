using ReviewRumble.Models;

namespace ReviewRumble.Business;

public interface IPullRequestBal
{
    public Task<PullRequestViewModel> AddAsync(NewPullRequest newPullRequest, string authorName);
    public Task<List<PullRequestViewModel>> GetAllAsync();
}

using ReviewRumble.Models;

namespace ReviewRumble.Business;

public interface IPullRequestBal
{
    public Task<PullRequestViewModel> Add(NewPullRequest newPullRequest, string authorName);
}

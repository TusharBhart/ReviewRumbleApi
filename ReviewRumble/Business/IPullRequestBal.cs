using ReviewRumble.Models;

namespace ReviewRumble.Business;

public interface IPullRequestBal
{
    public Task<PrReviewers> GetReviewers(GetPullRequest pullRequest);
}

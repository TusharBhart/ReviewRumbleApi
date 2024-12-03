using ReviewRumble.Models;

namespace ReviewRumble.Repository;
public interface IDataRepository
{
    public Task<List<PullRequest>> GetAllPullRequestsAsync();
    public Task<PullRequest?> GetPullRequestByIdAsync(int id);
    public Task AddPullRequestAsync(PullRequest pullRequest);
    public Task<List<Reviewer>> GetAllReviewersAsync();
    public Task<Reviewer?> GetReviewerByIdAsync(int id);

    public Task<Reviewer?> GetReviewerWithLeastPendingPrsAsync(List<string> reviewers);
    public Task AddReviewerAsync(Reviewer reviewer);
    public Task AssignReviewerToPullRequestAsync(PullRequest newPullRequest, List<Reviewer> reviewer);
}


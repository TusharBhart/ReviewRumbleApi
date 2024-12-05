using ReviewRumble.Models;

namespace ReviewRumble.Repository;
public interface IDataRepository
{
    public Task<List<PullRequest>> GetAllPullRequestsAsync();
    public Task<PullRequest?> GetPullRequestByAuthorAsync(string author);
    public Task AddPullRequestAsync(PullRequest pullRequest, List<User> reviewers);
    //public Task<List<User>> GetAllReviewersAsync();
    public Task<User?> GetUserByUserNameAsync(string userName);
    public Task UpdateUsersInProgressCountAsync(List<User> users);
    public Task<User?> GetReviewerWithLeastInProgressCountAsync(List<string> reviewers, HashSet<string> restrictedReviewers);
    public Task AddReviewerAsync(User reviewer);
}


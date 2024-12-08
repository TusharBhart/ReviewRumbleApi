using ReviewRumble.Models;

namespace ReviewRumble.Repository;
public interface IDataRepository
{
    public Task<List<PullRequest>> GetAllPullRequestsAsync();
    public Task AddPullRequestAsync(PullRequest pullRequest, List<User> reviewers);
    public Task<List<PullRequest>> GetAssignedPullRequestsAsync(int userName);
    public Task<List<PullRequest>> GetMyPullRequestsAsync(int userName);
    public Task<User?> GetUserByUserNameAsync(string userName);
    public Task UpdateUsersInProgressCountAsync(List<User> users);
    public Task<User?> GetReviewerWithLeastInProgressCountAsync(List<string> reviewers, HashSet<string> restrictedReviewers);
    public Task UpdateReviewerStatusAsync(int id, ReviewerStatusEnum status);
    public Task UpdatePullRequestStatusAsync(int pullRequestId, int userId, ReviewStatusEnum status);
    public Task<List<User>> GetUsersOrderedByTotalReviewCountAsync();
    public Task AddUserAsync(User user);
}
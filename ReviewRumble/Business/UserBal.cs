using ReviewRumble.Models;
using ReviewRumble.Repository;
using ReviewRumble.Utils;

namespace ReviewRumble.Business;

public class UserBal : IUserBal
{
    private readonly ReviewsConfigManager reviewsConfigManager;
    private readonly IDataRepository dataRepository;

    public UserBal(ReviewsConfigManager reviewsConfigManager, IDataRepository dataRepository)
    {
        this.reviewsConfigManager = reviewsConfigManager;
        this.dataRepository = dataRepository;
    }
    public async Task<UserInfo> GetUserAsync(string author)
    {
        var user =  await dataRepository.GetUserByUserNameAsync(author);
        return new UserInfo
        {
            Id = user.Id,
            Username = user.Username,
            Status = user.Status.ToString(),
            TotalReviewCount = user.TotalReviewCount
        };
    }

    public async Task<List<PullRequestViewModel>> GetAssignedPullRequestsAsync(int id)
    {
        var pullRequests = await dataRepository.GetAssignedPullRequestsAsync(id);

        return pullRequests.Select(pr => new PullRequestViewModel
        {
            Url = pr.Url,
            Author = pr.Author?.Username ?? string.Empty,
            AddedDate = pr.AddedDate,
            Repository = pr.Repository,
            Reviewers = pr.Reviewers.Select(r => new ReviewerInfo 
                {
                    Id = r.Reviewer.Id,
                    Username = r.Reviewer.Username
            })
                .ToList(),
            Status = pr.Reviewers.Where(r => r.Reviewer.Id == id).FirstOrDefault().ReviewStatus.ToString()
        }).OrderBy(pr => reviewsConfigManager.GetRepositoryPriority(pr.Repository)).ToList();
    }

    public async Task<List<PullRequestViewModel>> GetMyPullRequestsAsync(int id)
    {
        var pullRequests = await dataRepository.GetMyPullRequestsAsync(id);

        return pullRequests.Select(pr => new PullRequestViewModel
        {
            Url = pr.Url,
            Author = pr.Author?.Username ?? string.Empty,
            AddedDate = pr.AddedDate,
            Repository = pr.Repository,
            Reviewers = pr.Reviewers.Select(r => new ReviewerInfo
            {
                Id = r.Reviewer.Id,
                Username = r.Reviewer.Username
            })
                .ToList(),
            Status = ReviewStatusEnum.Open.ToString()
        }).ToList();
    }

    public async Task UpdateStatusAsync(int id, ReviewerStatusEnum status)
    {
        await dataRepository.UpdateReviewerStatusAsync(id, status);
    }

    public async Task<List<UserInfo>> GetLeaderboard()
    {
        var users = await dataRepository.GetUsersOrderedByTotalReviewCountAsync();
        return users.Select(u => new UserInfo
        {
            Id = u.Id,
            Username = u.Username,
            Status = u.Status.ToString(),
            TotalReviewCount = u.TotalReviewCount,
        }).ToList();
    }
}
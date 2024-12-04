using ReviewRumble.Models;
using ReviewRumble.Repository;
using ReviewRumble.Utils;

namespace ReviewRumble.Business;

public class PullRequestBal : IPullRequestBal
{
    private readonly ReviewsConfigManager reviewsConfigManager;
    private readonly IDataRepository dataRepository;

    public PullRequestBal(ReviewsConfigManager reviewsConfigManager, IDataRepository dataRepository)
    {
        this.reviewsConfigManager = reviewsConfigManager;
        this.dataRepository = dataRepository;
    }

    public async Task<PullRequestViewModel> Add(NewPullRequest pullRequest, string authorName)
    {
        var repository = GetRepoName(pullRequest.Url);
        var groups = reviewsConfigManager.GetRepositoryGroups(repository);

        HashSet<string> restrictedReviewers = [authorName]; 
        var reviewers = groups.Select(group => GetOptimalReviewerAsync(group, restrictedReviewers).Result).ToList();
        var author =  await dataRepository.GetUserByUserNameAsync(authorName);

        var newPullRequest = new PullRequest
        {
            Url = pullRequest.Url,
            AddedDate = DateTime.UtcNow,
            Repository = repository,
            AuthorId = author?.Id ?? 0,
            //Author = author,
            PrimaryReviewerId = reviewers[0]?.Id ?? 0,
            //PrimaryReviewer = reviewers[0],
            SecondaryReviewerId = reviewers.Count > 1 ? reviewers[1]?.Id ?? 0 : 0,
            //SecondaryReviewer= reviewers.Count > 1 ? reviewers[1] : null
        };

        await dataRepository.AddPullRequestAsync(newPullRequest);
        
        reviewers.ForEach(reviewer =>
            reviewer.InProgressReviewCount += 1);
        await dataRepository.UpdateUsersInProgressCountAsync(reviewers);

        return new PullRequestViewModel
        {
            Url = newPullRequest.Url,
            Author = newPullRequest.Author?.Username ?? string.Empty,
            AddedDate = newPullRequest.AddedDate,
            Repository = newPullRequest.Repository,
            PrimaryReviewer = newPullRequest.PrimaryReviewer?.Username ?? string.Empty,
            SecondaryReviewer = newPullRequest.SecondaryReviewer?.Username ?? string.Empty,
            Status = ReviewStatusEnum.Open.ToString()
        };
    }

    #region Private functions

    private static string GetRepoName(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty.");

        var parts = url.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (!parts[1].Contains("github.com"))
            throw new ArgumentException("Invalid GitHub URL format.");

        return parts[3];
    }

    private async Task<User?> GetOptimalReviewerAsync(string group, HashSet<string> restrictedReviewers)
    {
        var reviewers = reviewsConfigManager.GetGroupReviewers(group);
        if (reviewers == null)
        {
            throw new ArgumentException("reviewers is not null");
        }

        var reviewer = await dataRepository.GetReviewerWithLeastInProgressCountAsync(reviewers, restrictedReviewers);

        if (reviewer is not null) restrictedReviewers.Add(reviewer.Username);
        return reviewer;
    }

    #endregion
}
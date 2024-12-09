using ReviewRumble.Models;
using ReviewRumble.Repository;
using ReviewRumble.Utils;
using System;
using Azure.Core;

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

    public async Task<PullRequestViewModel> AddAsync(NewPullRequest pullRequest, string authorName)
    {
        var repository = GetRepoName(pullRequest.Url);
        var groups = reviewsConfigManager.GetRepositoryGroups(repository);

        HashSet<string> restrictedReviewers = [authorName];
        var reviewers = groups.Select(group => GetOptimalReviewerAsync(group, restrictedReviewers).Result).ToList();
        var author = await dataRepository.GetUserByUserNameAsync(authorName);

        var newPullRequest = new PullRequest
        {
            Url = pullRequest.Url,
            AddedDate = DateTime.UtcNow,
            Repository = repository,
            AuthorId = author?.Id ?? 0
        };

        await dataRepository.AddPullRequestAsync(newPullRequest, reviewers);

        reviewers.ForEach(reviewer =>
            reviewer.InProgressReviewCount += 1);
        await dataRepository.UpdateUsersInProgressCountAsync(reviewers);

        return new PullRequestViewModel
        {
            id = newPullRequest.Id,
            Title = newPullRequest.Url,
            Url = newPullRequest.Url,
            Author = new ReviewerInfo
            {
                Id = newPullRequest.Author?.Id ?? 0,
                Username = newPullRequest.Author?.Username ?? string.Empty,
            },
            AddedDate = newPullRequest.AddedDate,
            Repository = newPullRequest.Repository,
            Reviewers = newPullRequest.Reviewers.Select(r => new ReviewerInfo 
                {
                    Id = r.Reviewer.Id,
                    Username = r.Reviewer.Username
                })
                .ToList(),
            Status = ReviewStatusEnum.Open.ToString()
        };
    }

    public async Task<List<PullRequestViewModel>> GetAllAsync()
    {
        var pullRequests = await dataRepository.GetAllPullRequestsAsync();

        return pullRequests.Select(pr => new PullRequestViewModel
        {
            id = pr.Id,
            Title = pr.Url,
            Url = pr.Url,
            Author = new ReviewerInfo
            {
                Id = pr.Author?.Id ?? 0,
                Username = pr.Author?.Username ?? string.Empty,
            },
            AddedDate = pr.AddedDate,
            Repository = pr.Repository,
            Reviewers = pr.Reviewers.Select(r => new ReviewerInfo()
            {
                Id = r.Reviewer.Id,
                Username = r.Reviewer.Username,
            }).ToList(),
            Status = CalculateReviewStatus(pr.Reviewers.ToList()).ToString(),
        }).ToList();
    }

    public async Task UpdateStatusAsync(int pullRequestId, int userId, ReviewStatusEnum status)
    {
        await dataRepository.UpdatePullRequestStatusAsync(pullRequestId, userId, status);
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

    private ReviewStatusEnum CalculateReviewStatus(List<PullRequestReviewer> pullRequestReviewers)
    {
        if (pullRequestReviewers.Any(r => r.ReviewStatus == ReviewStatusEnum.RequestChanges))
        {
            return ReviewStatusEnum.RequestChanges;
        }

        if (pullRequestReviewers.Any(r => r.ReviewStatus == ReviewStatusEnum.InReview))
        {
            return ReviewStatusEnum.InReview;
        }

        return pullRequestReviewers.All(r => r.ReviewStatus == ReviewStatusEnum.Approved) ? ReviewStatusEnum.Approved : ReviewStatusEnum.Open;
    }

    #endregion
}
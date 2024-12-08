using Azure.Core;
using Microsoft.EntityFrameworkCore;
using ReviewRumble.Models;

namespace ReviewRumble.Repository;

public class DataRepository : IDataRepository
{
    private readonly ApiDbContext context;

    public DataRepository(ApiDbContext context)
    {
        this.context = context;
    }

    public async Task<List<PullRequest>> GetAllPullRequestsAsync()
    {
        return await context.PullRequests
            .Include(pr => pr.Author)
            .Include(pr => pr.Reviewers)
            .ThenInclude(r => r.Reviewer)
            .ToListAsync();
    }

    public async Task<PullRequest?> GetPullRequestByAuthorAsync(string author)
    {
        return await context.PullRequests
            .Include(pr => pr.Author)
            .Include(pr => pr.Reviewers)
            .FirstOrDefaultAsync(pr => pr.Author.Username == author);
    }

    public async Task AddPullRequestAsync(PullRequest pullRequest, List<User> reviewers)
    {
        foreach(var reviewer in reviewers)
        {
            pullRequest.Reviewers.Add(new PullRequestReviewer
            {
                ReviewerId = reviewer.Id
            });
        }

        await context.PullRequests.AddAsync(pullRequest);
        await context.SaveChangesAsync();
    }

    public async Task<List<PullRequest>> GetMyPullRequestsAsync(int authorId)
    {
        return await context.PullRequests
            .Where(pr => pr.AuthorId == authorId)
            .Include(pr => pr.Author) 
            .Include(pr => pr.Reviewers) 
            .ThenInclude(reviewer => reviewer.Reviewer)
            .ToListAsync();
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await context.Users
            .FirstOrDefaultAsync(user => user.Username == userName);
    }
    public async Task<List<PullRequest>> GetAssignedPullRequestsAsync(int id)
    {
        return await context.PullRequests
            .Where(pr => pr.Reviewers.Any(r => r.ReviewerId == id))
            .Include(pr => pr.Author) 
            .Include(pr => pr.Reviewers)
            .ThenInclude(reviewer => reviewer.Reviewer)
            .ToListAsync();
    }

    public async Task UpdateUsersInProgressCountAsync(List<User> users)
    {
        context.Users.UpdateRange(users);
        await context.SaveChangesAsync();
    }

    public async Task<User?> GetReviewerWithLeastInProgressCountAsync(List<string> reviewers, HashSet<string> restrictedReviewers)
    {
        return await context.Users
            .Where(user => user.Status != ReviewerStatusEnum.Inactive && !restrictedReviewers.Contains(user.Username) &&
                           reviewers.Contains(user.Username))
            .OrderBy(user => user.InProgressReviewCount)
            .Take(1)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateReviewerStatusAsync(int id, ReviewerStatusEnum status)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user != null) user.Status = status;

        await context.SaveChangesAsync();
    }

    public async Task UpdatePullRequestStatusAsync(int pullRequestId, int userId, ReviewStatusEnum newStatus)
    {
        var pullRequest = await context.PullRequests
            .Include(pr => pr.Reviewers)
            .ThenInclude(r => r.Reviewer)
            .FirstOrDefaultAsync(pr => pr.Id == pullRequestId);

        var pullRequestReviewer = pullRequest.Reviewers
            .FirstOrDefault(r => r.ReviewerId == userId);

        pullRequestReviewer.ReviewStatus = newStatus;

        if (newStatus == ReviewStatusEnum.RequestChanges)
        {
            pullRequest.Reviewers.Select(r => r.ReviewStatus = ReviewStatusEnum.RequestChanges);
        }

        if (newStatus == ReviewStatusEnum.Approved)
        {
            if (pullRequest.Reviewers.All(r => r.ReviewStatus == ReviewStatusEnum.Approved))
            {
                foreach (var reviewer in pullRequest.Reviewers)
                {
                    reviewer.Reviewer.TotalReviewCount++;

                    if (reviewer.Reviewer.InProgressReviewCount > 0)
                    {
                        reviewer.Reviewer.InProgressReviewCount--;
                    }
                }
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsersOrderedByTotalReviewCountAsync()
    {
        return await context.Users
            .OrderByDescending(u => u.TotalReviewCount)
            .ToListAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}
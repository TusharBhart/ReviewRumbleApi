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
            .Include(pr => pr.AssignedFirstReviewer)
            .Include(pr => pr.AssignedSecondReviewer)
            .ToListAsync();
    }

    public async Task<PullRequest?> GetPullRequestByIdAsync(int id)
    {
        return await context.PullRequests
            .Include(pr => pr.AssignedFirstReviewer)
            .Include(pr => pr.AssignedSecondReviewer)
            .FirstOrDefaultAsync(pr => pr.Id == id);
    }

    public async Task AddPullRequestAsync(PullRequest pullRequest)
    {
        context.PullRequests.Add(pullRequest);
        await context.SaveChangesAsync();
    }

    public async Task<List<Reviewer>> GetAllReviewersAsync()
    {
        return await context.Reviewers
            .Include(r => r.AssignedPullRequests)
            .ToListAsync();
    }

    public async Task<Reviewer?> GetReviewerByIdAsync(int id)
    {
        return await context.Reviewers
            .Include(r => r.AssignedPullRequests)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Reviewer?> GetReviewerWithLeastPendingPrsAsync(List<string> reviewers)
    {
        return await context.Reviewers
            .Where( r => reviewers.Contains(r.Username)) 
            .OrderBy(r => r.PendingPrsToReview)        
            .FirstOrDefaultAsync();
    }

    public async Task AddReviewerAsync(Reviewer reviewer)
    {
        context.Reviewers.Add(reviewer);
        await context.SaveChangesAsync();
    }

    //public async Task AssignReviewerToPullRequestAsync(int pullRequestId, int reviewerId, bool isFirstReviewer)
    //{
    //    var pullRequest = await GetPullRequestByIdAsync(pullRequestId);
    //    var reviewer = await GetReviewerByIdAsync(reviewerId);

    //    if (pullRequest == null || reviewer == null)
    //    {
    //        throw new ArgumentException("PullRequest or Reviewer not found.");
    //    }

    //    if (isFirstReviewer)
    //    {
    //        pullRequest.AssignedFirstReviewerId = reviewerId;
    //        pullRequest.AssignedFirstReviewer = reviewer;
    //    }
    //    else
    //    {
    //        pullRequest.AssignedSecondReviewerId = reviewerId;
    //        pullRequest.AssignedSecondReviewer = reviewer;
    //    }

    //    context.PullRequests.Update(pullRequest);
    //    await context.SaveChangesAsync();

    //}
    public async Task AssignReviewerToPullRequestAsync(PullRequest newPullRequest, List<Reviewer> reviewers)
    {
        if (reviewers.Count > 0)
        {
            newPullRequest.AssignedFirstReviewerId = reviewers[0].Id;
            newPullRequest.AssignedFirstReviewer = reviewers[0];
        }
        if(reviewers.Count > 1)
        {
            newPullRequest.AssignedSecondReviewerId = reviewers[1].Id;
            newPullRequest.AssignedSecondReviewer = reviewers[1];
        }

        context.PullRequests.Add(newPullRequest);
        await context.SaveChangesAsync();
    }

}
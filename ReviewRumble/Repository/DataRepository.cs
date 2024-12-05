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

    //public async Task<List<User>> GetAllReviewersAsync()
    //{
    //    return await context.Users
    //        .Include(r => r.AssignedPullRequests)
    //        .ToListAsync();
    //}

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await context.Users
            .FirstOrDefaultAsync(user => user.Username == userName);
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

    public async Task AddReviewerAsync(User reviewer)
    {
        context.Users.Add(reviewer);
        await context.SaveChangesAsync();
    }
}
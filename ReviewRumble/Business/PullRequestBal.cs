using System.Data;
using ReviewRumble.Models;
using ReviewRumble.Repository;

namespace ReviewRumble.Business;

public class PullRequestBal : IPullRequestBal
{
    private readonly ConfigurationService configurationService;
    private readonly IDataRepository dataRepository;
    public PullRequestBal(ConfigurationService configurationService, IDataRepository dataRepository)
    {
        this.configurationService = configurationService;
        this.dataRepository = dataRepository;
    }

    public async Task<PrReviewers> GetReviewers(GetPullRequest pullRequest)
    {
        var repo = GetRepoName(pullRequest.PullRequestUrl);

        var groups = configurationService.GetRepositoryGroups(repo);

        var reviewers = groups.Select(g => GetOptimalReviewers(g).Result).ToList();

        var newPr = new PullRequest
        {
            Author = pullRequest.Author,
            Url = pullRequest.PullRequestUrl,
            CreatedDate = DateTime.UtcNow,
            Repository = repo
        };

        await dataRepository.AssignReviewerToPullRequestAsync(newPr, reviewers);

        return new PrReviewers
        {
            Reviewers = reviewers
        };
    }

    #region Private functions

    private static string GetRepoName(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty.");

        var parts = url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (!parts[1].Contains("github.com"))
            throw new ArgumentException("Invalid GitHub URL format.");

        return parts[3];
    }

    private async Task<Reviewer?> GetOptimalReviewers(string group)
    {
        var reviewers = configurationService.GetGroupReviewers(group);
        return await dataRepository.GetReviewerWithLeastPendingPrsAsync(reviewers);
    }

    #endregion
}
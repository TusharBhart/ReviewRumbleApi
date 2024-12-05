namespace ReviewRumble.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public ReviewerStatusEnum Status { get; set; } = ReviewerStatusEnum.Active;
    public int TotalReviewCount { get; set; }
    public int InProgressReviewCount { get; set; }
    public List<PullRequest> MyPullRequests { get; set; } = [];
    public ICollection<PullRequestReviewer> AssignedReviews { get; set; } = new List<PullRequestReviewer>();
}
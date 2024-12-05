namespace ReviewRumble.Models;

public class PullRequestReviewer
{
    public int PullRequestId { get; set; }
    public PullRequest PullRequest { get; set; } = null!;

    public int ReviewerId { get; set; }
    public User Reviewer { get; set; } = null!;

    public ReviewStatusEnum ReviewStatus { get; set; } = ReviewStatusEnum.Open;
}
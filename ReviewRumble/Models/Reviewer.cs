namespace ReviewRumble.Models;

public class Reviewer
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int TotalPrsReviewed { get; set; } 
    public int PendingPrsToReview { get; set; }
    public List<PullRequest> AssignedPullRequests { get; set; } = [];
}
namespace ReviewRumble.Models;

public class PullRequest
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string Repository { get; set; }
    public DateTime AddedDate { get; set; }
    public int AuthorId { get; set; }
    public User? Author { get; set; }
    public ICollection<PullRequestReviewer> Reviewers { get; set; } = new List<PullRequestReviewer>();
}
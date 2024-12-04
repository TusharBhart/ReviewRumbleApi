namespace ReviewRumble.Models;

public class PullRequest
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string Repository { get; set; }
    public ReviewStatusEnum PrimaryReviewerStatus { get; set; } = ReviewStatusEnum.Open;
    public ReviewStatusEnum SecondaryReviewerStatus { get; set; } = ReviewStatusEnum.Open;
    public DateTime AddedDate { get; set; }
    public int AuthorId { get; set; }
    public User? Author { get; set; }
    public int PrimaryReviewerId { get; set; }
    public User? PrimaryReviewer { get; set; }
    public int SecondaryReviewerId { get; set; }
    public User? SecondaryReviewer { get; set; }
}
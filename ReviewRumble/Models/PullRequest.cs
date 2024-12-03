namespace ReviewRumble.Models;

public class PullRequest
{
    public int Id { get; set; } 
    public string Author { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Repository { get; set; } = string.Empty;
    public PrStatusEnum FirstReviewerStatus { get; set; } = PrStatusEnum.InReview;
    public PrStatusEnum SecondReviewerStatus { get; set; } = PrStatusEnum.InReview;
    public DateTime CreatedDate { get; set; } 

    public int AssignedFirstReviewerId{ get; set; }
    public Reviewer? AssignedFirstReviewer { get; set; }

    public int AssignedSecondReviewerId { get; set; }
    public Reviewer? AssignedSecondReviewer { get; set; }
}
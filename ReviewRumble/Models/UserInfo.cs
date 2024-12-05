namespace ReviewRumble.Models;

public class UserInfo
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public ReviewerStatusEnum Status { get; set; } = ReviewerStatusEnum.Active;
    public int TotalReviewCount { get; set; }
    public int InProgressReviewCount { get; set; }
}


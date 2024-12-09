namespace ReviewRumble.Models;

public class UserInfo
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Status { get; set; }
    public int TotalReviewCount { get; set; }
}
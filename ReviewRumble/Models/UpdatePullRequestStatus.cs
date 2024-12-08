namespace ReviewRumble.Models;

public class UpdatePullRequestStatus
{
    public int Id { get; set; } 
    public ReviewStatusEnum Status { get; set; }
}
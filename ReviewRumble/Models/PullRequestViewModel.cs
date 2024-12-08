namespace ReviewRumble.Models;

public class PullRequestViewModel
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Url { get; set; }
    public string Repository { get; set; }
    public ReviewStatusEnum Status { get; set; }
    public DateTime AddedDate { get; set; }
    public object Reviewers { get; set; }
}
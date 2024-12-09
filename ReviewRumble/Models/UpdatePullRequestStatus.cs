using System.Text.Json.Serialization;

namespace ReviewRumble.Models;

public class UpdatePullRequestStatus
{
    [JsonConverter(typeof(JsonStringEnumConverter<ReviewStatusEnum>))]
    public ReviewStatusEnum Status { get; set; }
}
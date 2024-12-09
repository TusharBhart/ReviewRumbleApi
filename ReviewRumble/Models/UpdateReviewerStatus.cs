using System.Text.Json.Serialization;

namespace ReviewRumble.Models;

public class UpdateReviewerStatus
{
    [JsonConverter(typeof(JsonStringEnumConverter<ReviewerStatusEnum>))]
    public ReviewerStatusEnum Status { get; set; }
}
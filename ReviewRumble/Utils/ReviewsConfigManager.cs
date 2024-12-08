using Newtonsoft.Json;
using ReviewRumble.Models;

namespace ReviewRumble.Utils;

public class ReviewsConfigManager
{
    private Dictionary<string, RepositoryConfig> repositoryGroups = new();
    private Dictionary<string, List<string>> groupReviewers = new();
    private const string JsonFilePath = "ReviewersConfig.json";

    public ReviewsConfigManager()
    {
        LoadRepositoryReviewers();
    }

    public List<string> GetRepositoryGroups(string repo)
    {
        var repositoryConfig = repositoryGroups.TryGetValue(repo.ToLower(), out var group)
            ? group
            : repositoryGroups["default"];

        return repositoryConfig.Reviewers;
    }

    public int GetRepositoryPriority(string repo)
    {
        return repositoryGroups.TryGetValue(repo.ToLower(), out var group)
            ? group.Priority
            : repositoryGroups["default"].Priority;
    }

    public List<string>? GetGroupReviewers(string group)
    {
        groupReviewers.TryGetValue(group, out var reviewers);
        return reviewers;
    }

    private void LoadRepositoryReviewers()
    {
        var jsonContent = File.ReadAllText(JsonFilePath);
        var config = JsonConvert.DeserializeObject<ReviewersConfig>(jsonContent);

        if (config?.Groups != null)
        {
            groupReviewers = config.Groups;
        }

        if (config?.Repositories != null)
        {
            repositoryGroups = config.Repositories;
        }
    }
}


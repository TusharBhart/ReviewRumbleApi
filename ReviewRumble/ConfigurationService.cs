using Newtonsoft.Json;

namespace ReviewRumble;
public class ConfigurationService
{
    private Dictionary<string, List<string>> repositoryGroups;
    private Dictionary<string, List<string>> groupReviewers;
    private readonly string jsonFilePath = "bin/Debug/net8.0/Models/Groups.json";
    private FileSystemWatcher fileWatcher;

    public ConfigurationService()
    {
        LoadRepositoryReviewers();
        SetupFileWatcher();
    }

    public List<string> GetRepositoryGroups(string repo)
    {
        return repositoryGroups.TryGetValue(repo, out var group)
            ? group
            : repositoryGroups["all_repos"];
    }

    public List<string> GetGroupReviewers(string group)
    {
        groupReviewers.TryGetValue(group, out var reviewers);
        return reviewers;
    }

    private void LoadRepositoryReviewers()
    {
            string jsonContent = File.ReadAllText(jsonFilePath);

            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonContent);

            if (jsonObject.ContainsKey("groups"))
            {
                groupReviewers = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonObject["groups"].ToString());
            }

            if (jsonObject.ContainsKey("repos"))
            {
                repositoryGroups = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonObject["repos"].ToString());
            }
    }

    private void SetupFileWatcher()
    {
        fileWatcher = new FileSystemWatcher("C:/Users/tbharat/source/repos/ReviewRumble/ReviewRumble/bin/Debug/net8.0/Models")
        {
            Filter = "Groups.json",
            NotifyFilter = NotifyFilters.Attributes
                                       | NotifyFilters.CreationTime
                                       | NotifyFilters.DirectoryName
                                       | NotifyFilters.FileName
                                       | NotifyFilters.LastAccess
                                       | NotifyFilters.LastWrite
                                       | NotifyFilters.Security
                                       | NotifyFilters.Size
        };

        fileWatcher.Changed += (_, _) =>
        {
            LoadRepositoryReviewers();
        };

        fileWatcher.EnableRaisingEvents = true;
    }
}


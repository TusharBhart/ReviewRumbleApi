//using Microsoft.AspNetCore.DataProtection.KeyManagement;

//namespace ReviewRumble;

//public class ReviewRules
//{
//    private Dictionary<string, List<string>> groups = new()
//    {
//        { "angular-developer", ["Ayushi-Rohela", "adarshkumar-wg", "gauravpreet-wg"] },
//        { "nonprod-admins", ["durgeshkumar14", "vverma04", "atin-singla", "saurabhb-watchguard"] },
//        { "prod-admins", ["durgeshkumar14", "sviru"] },
//        { "prod-access", ["vverma04", "saurabhb-watchguard", "atin-singla", "adarshkumar-wg", "sauravjaiswal"] },
//        {
//            "br-api-reviewers",
//            ["saurabhb-watchguard", "sauravjaiswal", "atin-singla", "adarshkumar-wg", "arunrfid", "alokwg"]
//        },
//        {
//            "tier-1-reviewers",
//            [
//                "saurabhb-watchguard", "sauravjaiswal", "atin-singla", "adarshkumar-wg", "arunrfid", "vverma04",
//                "durgeshkumar14"
//            ]
//        },
//        {
//            "tier-2-reviewers",
//            [
//                "vsingla10", "alokwg", "gauravpreet-wg", "afeefashraf-wg", "sakshammittal-wg", "srana-wg",
//                "tusharbhart-wg",
//                "akshayWG", "Ayushi-Rohela", "snehagoyal-wg", "AkhilSharma28", "kunalagarwal05", "WG-SSharma"
//            ]
//        },
//        {
//            "all-reviewers",
//            [
//                "vsingla10", "alokwg", "gauravpreet-wg", "afeefashraf-wg", "sakshammittal-wg", "srana-wg",
//                "tusharbhart-wg",
//                "akshayWG", "Ayushi-Rohela", "snehagoyal-wg", "AkhilSharma28", "kunalagarwal05", "WG-SSharma",
//                "saurabhb-watchguard", "sauravjaiswal", "atin-singla", "adarshkumar-wg", "arunrfid", "vverma04",
//                "durgeshkumar14"
//            ]
//        }
//    };

//    private Dictionary<string, List<string>> repositoryGroups = new()
//    {
//        { "myproductsui", ["angular-developer"] },
//        { "businessruleapi", ["br-api-reviewers", "br-api-reviewers"] },
//        { "prodappconfigurations", ["prod-admins", "prod-access"] },
//        { "nonprodappconfigurations", ["nonprod-admins", "all-reviewers"] },
//        { "all_repos", ["tier-1-reviewers", "tier-2-reviewers"] }
//    };

//    public List<string> GetRepositoryGroups(string repositoryName)
//    {
//        return repositoryGroups.TryGetValue(repositoryName, out var reviewer)
//            ? reviewer
//            : repositoryGroups["all_repos"];
//    }

//}


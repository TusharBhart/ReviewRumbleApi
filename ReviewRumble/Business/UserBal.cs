using ReviewRumble.Models;
using ReviewRumble.Repository;
using ReviewRumble.Utils;

namespace ReviewRumble.Business;

public class UserBal : IUserBal
{
    private readonly IDataRepository dataRepository;

    public UserBal(IDataRepository dataRepository)
    {
        this.dataRepository = dataRepository;
    }
    public async Task<UserInfo> GetUser(string author)
    {
        var user =  await dataRepository.GetUserByUserNameAsync(author);
        return new UserInfo
        {
            Id = user.Id,
            Username = user.Username,
            Status = user.Status,
            InProgressReviewCount = user.InProgressReviewCount,
            TotalReviewCount = user.TotalReviewCount
        };
    }
}
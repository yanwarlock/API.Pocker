using API.Pocker.Models.ManageAccounts;
namespace API.Pocker.Models.User
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AccountModel Account { get; set; }

    }
}

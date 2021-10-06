using API.Pocker.Models.ManageAccounts;
namespace API.Pocker.Models.User
{
    public class UserProfileRequest
    {
        public string Name { get; set; }
        public CreateAccountRequest Account { get; set; }
    }
}

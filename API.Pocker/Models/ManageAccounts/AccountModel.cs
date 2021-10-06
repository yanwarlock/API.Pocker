using System.Collections.Generic;

namespace API.Pocker.Models.ManageAccounts
{
    public class AccountModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IList<string> Role { get; set; }

        public AccountModel()
        {
            Role = new List<string>();
        }
    }
}

using API.Pocker.Models.ManageAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Models.User
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AccountModel Account { get; set; }

    }
}

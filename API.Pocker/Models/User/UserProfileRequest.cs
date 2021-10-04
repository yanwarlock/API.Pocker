using API.Pocker.Models.ManageAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Models.User
{
    public class UserProfileRequest
    {
        public string Name { get; set; }
        public CreateAccountRequest CreateAccountRequest { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Models.ManageAccounts
{
    public class AuthenticationModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Role { get; set; }
        public DateTime Created { get; set; }
        public string JwtToken { get; set; }
        public RefreshTokenModel RefreshToken { get; set; }
    }
}

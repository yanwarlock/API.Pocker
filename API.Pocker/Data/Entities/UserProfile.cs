using API.Pocker.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace API.Pocker.Data.Entities
{
    public class UserProfile
    {
        public string Id { get; set; }
        public  string Name { get; set; }
        public IdentityUser UserIdentity { get; set; }
        public string UserIdentityId { get; set; }
        public ICollection<UserProfileHistory> UserHistorys { get; set; }
        public UserProfile()
        {
            Id = GenerateBy.GenerateByUid();
        }
    }
}

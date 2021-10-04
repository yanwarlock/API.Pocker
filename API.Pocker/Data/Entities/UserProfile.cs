using API.Pocker.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace API.Pocker.Data.Entities
{
    public class UserProfile
    {
        public string Id { get; set; }
        public  string Name { get; set; }
        public string IdUserIdentity { get; set; }
        public UserProfile()
        {
            Id = GenerateBy.GenerateByUid();
        }
    }
}

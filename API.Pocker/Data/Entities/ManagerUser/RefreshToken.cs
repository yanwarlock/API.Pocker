using API.Pocker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Data.Entities.ManagerUser
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string AccountId { get; set; }
        public RefreshToken()
        {
            Id = GenerateBy.GenerateByUid();
        }
    }
}

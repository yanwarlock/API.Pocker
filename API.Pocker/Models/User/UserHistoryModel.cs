using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Models.User
{
    public class UserHistoryModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

    }
}

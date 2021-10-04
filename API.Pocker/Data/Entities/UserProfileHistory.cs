using API.Pocker.Helpers;
using System;
using System.Collections.Generic;

namespace API.Pocker.Data.Entities
{
    public class UserProfileHistory
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public DateTime Date { get; set; }

        // public ICollection<Wishes> Votes { get; set; }
        public UserProfileHistory()
        {
            Id = GenerateBy.GenerateByUid();
        }
    }
}

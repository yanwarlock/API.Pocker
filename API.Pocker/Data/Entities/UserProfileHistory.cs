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
        public string UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public UserProfileHistory()
        {
            Id = GenerateBy.GenerateByUid();
            Date = DateTime.Now;
        }
    }
}

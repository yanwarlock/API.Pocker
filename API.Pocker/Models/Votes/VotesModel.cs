using API.Pocker.Data.Entities;
using API.Pocker.Models.Cards;
using API.Pocker.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Models.Votes
{
    public class VotesModel
    {
        public string Id { get; set; }
        public UserProfileModel UserProfile { get; set; }
        public CardsModel Cards { get; set; }
        public UserHistoryModel UserHistory { get; set; }
     
    }
}

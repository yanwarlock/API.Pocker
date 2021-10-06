
using API.Pocker.Models.Cards;
using API.Pocker.Models.User;

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

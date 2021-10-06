using API.Pocker.Helpers;

namespace API.Pocker.Data.Entities
{
    public class Votes
    {
        public string Id { get; set; }
        public UserProfile UserProfile { get; set; }
        public string UserProfileId { get; set; }
        public Card Cards { get; set; }
        public string  CardsId { get; set; }
        public UserProfileHistory UserHistory { get; set; }
        public string UserHistoryId { get; set; }
        public Votes()
        {
            Id = GenerateBy.GenerateByUid();
        }
    }
}

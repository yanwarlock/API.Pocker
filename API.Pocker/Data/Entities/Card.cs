using API.Pocker.Helpers;

namespace API.Pocker.Data.Entities
{
    public class Card
    {
        public string Id { get; set; }
        public int Value { get; set; }

        public Card()
        {
            Id = GenerateBy.GenerateByUid();
        }
    }
}

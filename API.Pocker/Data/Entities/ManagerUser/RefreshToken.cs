using API.Pocker.Helpers;

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

using System;
namespace API.Pocker.Helpers
{
    public class GenerateBy
    {
        public static string GenerateByUid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

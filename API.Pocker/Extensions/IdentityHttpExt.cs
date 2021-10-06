using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace API.Pocker.Extensions
{
    public static class IdentityHttpExt
    {
        public static async Task WriteJsonAsync(this HttpResponse response, int statusCode, object body = null)
        {
            if (response.HasStarted)
            {
                return;
            }

            response.Clear();
            response.StatusCode = statusCode;

            if (body != null)
            {
                response.ContentType = "application/json";
                var json = JsonConvert.SerializeObject(body, JsonOptions);
                await response.WriteAsync(json);
            }
        }

        private static readonly JsonSerializerSettings JsonOptions = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string GetUserEmail(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Email);
            return claim.Value;
        }
    }
}


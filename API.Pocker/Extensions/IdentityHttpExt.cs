using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}


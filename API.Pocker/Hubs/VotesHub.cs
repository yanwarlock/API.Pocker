using API.Pocker.Models.Votes;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Pocker.Hubs
{
    public class VotesHub : Hub
    {
    }

    public static class VotesHubExt
    {
        public static async Task SendAllVotes(this IHubContext<VotesHub> context, IEnumerable<VotesModel> wishes)
        {
            await context.Clients.All.SendAsync("AllVotes", wishes);
        }
    }
}

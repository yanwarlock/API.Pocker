using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Pocker.Hubs;
using API.Pocker.Models;
using API.Pocker.Models.Votes;
using API.Pocker.Services;
using API.Pocker.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace API.Pocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VotesController : ControllerBase
    {
        public VotesController()
        {

        }
        [HttpPost("CreateVotes")]
        public async Task<ResponseAPI<VotesModel>> CreateVotes(VotesRequest request)
        {
            var service = Request.HttpContext.RequestServices.GetService<VotesService>();
            var response = await service!.CreateAsync(request);

            var serviceRabbitMq = Request.HttpContext.RequestServices.GetService<AmqpService>();
            serviceRabbitMq.PublishMessage(response);

            var serviceVotes = Request.HttpContext.RequestServices.GetService<VotesService>();
            var responseVotes = await service!.GetAllAsync();

            var hubContext = Request.HttpContext.RequestServices.GetService<IHubContext<VotesHub>>();
            await hubContext.SendAllVotes(
              //TODO: get all votes
              responseVotes.Data
            );


            return response;
        }
        [HttpDelete("DeleteVotes")]
        public async Task<ResponseAPI> DeleteVotes(string request)
        {
            var service = Request.HttpContext.RequestServices.GetService<VotesService>();
            var response = await service!.DeleteAsync(request);
            return response;
        }
        [HttpGet("GetAllVotes")]
        public async Task<ResponseAPI<IList<VotesModel>>> GetAllVotes()
        {
            var service = Request.HttpContext.RequestServices.GetService<VotesService>();
            var response = await service!.GetAllAsync();
            return response;
        }

        [HttpGet("notify-votes")]
        [AllowAnonymous]
        public async Task<IActionResult> NotifyVotes()
        {
            var hubContext = Request.HttpContext.RequestServices.GetService<IHubContext<VotesHub>>();
            await hubContext.SendAllVotes(
                //TODO: get all votes
                new VotesModel[]
                {
                    new VotesModel { Id = Guid.NewGuid().ToString("D") }
                }
            );

            return Ok();
        }
    }
}
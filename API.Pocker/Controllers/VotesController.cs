using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using API.Pocker.Hubs;
using API.Pocker.Models;
using API.Pocker.Models.Votes;
using API.Pocker.Services;
using API.Pocker.Services.Interfaces;
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
        private readonly IVotesService _votesService;
        public VotesController(VotesService votesService)
        {
            _votesService = votesService;
        }
        [HttpPost("CreateVotes")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(VotesRequest request)
        {
            try
            {
                if (request is null)
                    return BadRequest();

                var result = await _votesService.CreateAsync(request);

                var serviceRabbitMq = Request.HttpContext.RequestServices.GetService<AmqpService>();
                serviceRabbitMq.PublishMessage(result);


                var responseVotes = await _votesService!.GetAllAsync();

                var hubContext = Request.HttpContext.RequestServices.GetService<IHubContext<VotesHub>>();
                await hubContext.SendAllVotes(responseVotes.Data);

                return CreatedAtAction(nameof(Get), new { id = result.Data.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCodes.Status409Conflict, ex.Message });
            }
        }

        [HttpGet("GetVotes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(string request)
        {
            var result = await _votesService.GetAsync(request);
            if (result.Data is null)
                return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("DeleteVotes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string request)
        {
            var result = await _votesService.DeleteAsync(request);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }
        [HttpGet("GetAllVotes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllVotes()
        {
            var result = await _votesService.GetAllAsync();
            if (result.Data is null)
                return NotFound(result);
            return Ok(result);
        }

      /*  [HttpGet("notify-votes")]
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
        }*/
    }
}
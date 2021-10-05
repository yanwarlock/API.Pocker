using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using API.Pocker.Models;
using API.Pocker.Models.User;
using API.Pocker.Services;
using API.Pocker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Pocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserHistoryController : ControllerBase
    {
        private readonly IUserHistoryService _userHistoryService;
        public UserHistoryController(UserHistoryService userHistoryService)
        {
            _userHistoryService = userHistoryService;
        }

        [HttpPost("CreateUserHistory")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(UserHistoryRequest request)
        {
            try
            {
                if (request is null)
                    return BadRequest();
                var result = await _userHistoryService.CreateAsync(request);
                return CreatedAtAction(nameof(Get), new { id = result.Data.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCodes.Status409Conflict, ex.Message });
            }
        }
        [HttpGet("GetUserHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(string request)
        {
            var result = await _userHistoryService.GetAsyn(request);
            if (result.Data is null)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("GetAllUserHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get()
        {
            var result = await _userHistoryService.GetAllAsync();
            if (result.Data is null)
                return NotFound(result);
            return Ok(result);
        }
    }
}
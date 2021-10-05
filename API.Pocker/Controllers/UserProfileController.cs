using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Pocker.Models;
using API.Pocker.Models.User;
using Microsoft.Extensions.DependencyInjection;
using API.Pocker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Pocker.Services.Interfaces;
using System.Net.Mime;

namespace API.Pocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        public UserProfileController(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        [HttpPost("CreateUserProfile")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(UserProfileRequest request)
        {
            try
            {
                if (request is null)
                    return BadRequest();
                var result = await _userProfileService.CreateAsync(request);
                if(!result.Succeeded)
                    return BadRequest(new { StatusCodes.Status409Conflict, result.Message});
                return CreatedAtAction(nameof(Get), new { id = result.Data.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCodes.Status409Conflict, ex.Message });
            }
        }

        [HttpGet("UserProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(string request)
        {
            var result = await _userProfileService.GetAsync(request);
            if (result.Data is null)
                return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("DeleteUserProfile")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string request)
        {
            var result = await _userProfileService.DeleteAsync(request);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("GetAllUserProfile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get()
        {
            var result = await _userProfileService.GetAllAsync();
            if (result.Data is null)
                return NotFound(result);
            return Ok(result);

        }
    }
}
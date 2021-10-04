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
        public async Task<ActionResult> CreateUserProfile(UserProfileRequest request)
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
        [HttpGet("{id:required}")]
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
        public async Task<ResponseAPI> DeleteUserProfile(string request)
        {
            var service = Request.HttpContext.RequestServices.GetService<UserProfileService>();
            var response = await service!.DeleteAsync(request);
            return response;
        }
        [HttpGet("GetAllUserProfile")]
        [Authorize]
        public async Task<ResponseAPI<IList<UserProfileModel>>> GetAllUserProfile()
        {
            var service = Request.HttpContext.RequestServices.GetService<UserProfileService>();
            var response = await service!.GetAllAsync();
            return response;
        }
    }
}
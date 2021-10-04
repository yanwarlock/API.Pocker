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

namespace API.Pocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class UserProfileController : ControllerBase
    {
        public UserProfileController()
        {

        }
        [HttpPost("CreateUserProfile")]
        public async Task<ResponseAPI<UserProfileModel>> CreateUserProfile(UserProfileRequest request)
        {
            var service = Request.HttpContext.RequestServices.GetService<UserProfileService>();
            var response = await service!.CreateAsync(request);
            return response;
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
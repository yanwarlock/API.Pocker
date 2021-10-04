using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Pocker.Models;
using API.Pocker.Models.User;
using API.Pocker.Services;
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
        public UserHistoryController()
        {

        }

        [HttpPost("CreateUserHistory")]
        public async Task<ResponseAPI<UserHistoryModel>> CreateUserHistory(UserHistoryRequest request)
        {
            var service = Request.HttpContext.RequestServices.GetService<UserHistoryService>();
            var response = await service!.CreateAsync(request);
            return response;
        }
        [HttpGet("GetAllUserHistory")]
        public async Task<ResponseAPI<IList<UserHistoryModel>>> GetAllUserHistory()
        {
            var service = Request.HttpContext.RequestServices.GetService<UserHistoryService>();
            var response = await service!.GetAllAsync();
            return response;
        }
    }
}
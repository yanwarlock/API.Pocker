﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Pocker.Models;
using API.Pocker.Models.ManageAccounts;
using API.Pocker.Services.ManageAccounts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;

namespace API.Pocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageAccountsController : ControllerBase
    {
        private readonly IManageAccountService _manageAccountService;
        public ManageAccountsController(ManageAccountService manageAccountService)
        {
            _manageAccountService = manageAccountService;
        }

        [HttpPost("Authenticate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Authenticate(AuthenticationRequest request)
        {
            if (request is null)
                return BadRequest();
            var result = await _manageAccountService.AuthenticateAsync(request);
            if (result.Errors != null)
                return BadRequest(
                    new { StatusCodes.Status403Forbidden, result.Errors });
            return Ok(result);

        }
        [HttpGet("GetAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(string request)
        {
            var result = await _manageAccountService.GetAccountAsync(request);
            if (result.Data is null)
                return NotFound(result);
            return Ok(result);
        }


        [HttpPost("CreateAccount")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseAPI<AccountModel>> CreateAccount(CreateAccountRequest request)
        {
            var service = Request.HttpContext.RequestServices.GetService<ManageAccountService>();
            var response = await service.CreateAsync(request);
            return response;
        }
        [HttpDelete("DeleteAccount")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseAPI<AccountModel>> DeleteAccount(string request)
        {
            var service = Request.HttpContext.RequestServices.GetService<ManageAccountService>();
            var response = await service.DeleteAsync(request);
            return response;
        }

        [HttpPost("refresh_token")]
        public async Task<ResponseAPI<RefreshTokenModel>> RefreshToken(string refresh_token)
        {
            var service = Request.HttpContext.RequestServices.GetService<ManageAccountService>();
            var response = await service.RefreshToken(refresh_token);
            return response;
        }
    }
}
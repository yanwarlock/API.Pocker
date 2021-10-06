
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Pocker.Models;
using API.Pocker.Models.Cards;
using API.Pocker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Pocker.Services.Interfaces;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Principal;
using System.Security.Claims;
using API.Pocker.Extensions;
using API.Pocker.Models.User;

namespace API.Pocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardsController : ControllerBase
    {
        private readonly ICardsService _cardsService;
        private readonly IUserHistoryService _userHistoryService;
       public CardsController(CardsService cardsService, UserHistoryService userHistoryService)
        {
            _cardsService = cardsService;
            _userHistoryService = userHistoryService;
        }


        /// <summary>
        /// Crea una new Cards
        /// </summary>
        /// <param name="request"></param>
        /// <returns>retorna una Cards</returns>

        [HttpPost("CreateCards")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(CardsRequest request)
        {
            try
            {
                if (request is null)
                    return BadRequest();
                var result = await _cardsService.CreateAsync(request);
                //var userHistory = await _userHistoryService.CreateAsync(new UserHistoryRequest
                //{
                //    Email = User.GetUserEmail(),
                //    Description = $"Create Cards by Id: {result.Data.Id}"
                //});
                return CreatedAtAction(nameof(Get), new { id = result.Data.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCodes.Status409Conflict, ex.Message });
            } 
            
         }

        [HttpGet("GetCards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(string request)
        {
            var result = await _cardsService.GetAsyn(request);
            if (result.Data is null)
            {
                return NotFound(result);
            }
            //await _userHistoryService.CreateAsync(new UserHistoryRequest
            //{
            //    Email = User.GetUserEmail(),
            //    Description = $"Get Cards by Id: {result.Data.Id}"
            //});
            return Ok(result);
        }

        [HttpDelete("DeleteCards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string request)
        {
            var result = await _cardsService.DeleteAsync(request);
            if (!result.Succeeded)
                return NotFound(result);
            //await _userHistoryService.CreateAsync(new UserHistoryRequest
            //{
            //    Email = User.GetUserEmail(),
            //    Description = $"Delete Cards"
            //});
            return Ok(result);
        }

        [HttpGet("GetAllCards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get()
        {
            var result = await _cardsService.GetAllAsync();
            if (result.Data is null)
                return NotFound(result);

            //await _userHistoryService.CreateAsync(new UserHistoryRequest
            //{
            //    Email = User.GetUserEmail(),
            //    Description = $"Get All Cards"
            //});
            return Ok(result);
        }
    }
}
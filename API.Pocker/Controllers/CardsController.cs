
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

namespace API.Pocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardsController : ControllerBase
    {
        private readonly ICardsService _cardsService;
       public CardsController(CardsService cardsService)
        {
            _cardsService = cardsService;
        }

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
                return NotFound(result);
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
            return Ok(result);
        }
    }
}
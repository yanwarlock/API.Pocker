using System.Threading.Tasks;
using API.Pocker.Models.Cards;
using API.Pocker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Pocker.Services.Interfaces;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using System;

namespace API.Pocker.Controllers
{
    /// <summary>
    /// Cards Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardsController : ControllerBase
    {
        private readonly ICardsService _cardsService;
        private readonly IUserHistoryService _userHistoryService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardsService"></param>
        /// <param name="userHistoryService"></param>
       public CardsController(CardsService cardsService, UserHistoryService userHistoryService)
        {
            _cardsService = cardsService;
            _userHistoryService = userHistoryService;
        }


        /// <summary>
        /// Create a new cards.
        /// </summary>
        /// <param name="request">Cards to be created</param>
        /// <returns>Returns the cards created</returns>
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
        /// <summary>
        /// Get a given cards its identifier
        /// </summary>
        /// <param name="request">Cards indentifier</param>
        /// <returns>Returns a cards</returns>
        [HttpGet("GetCards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(string request)
        {
            var result = await _cardsService.GetAsync(request);
            if (result.Data is null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Delete a Cards
        /// </summary>
        /// <param name="request">Cards identifier</param>
        /// <returns>Returns true or false</returns>
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

        /// <summary>
        /// Get all cards
        /// </summary>
        /// <returns>Returns a collection of cards</returns>
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
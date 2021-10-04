using API.Pocker.Data;
using API.Pocker.Data.Entities;
using API.Pocker.Models;
using API.Pocker.Models.Cards;
using API.Pocker.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Services
{
    public class CardsService : ICardsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CardsService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResponseAPI<CardsModel>> CreateAsync(CardsRequest request)
        {
            if (request is null)
                return new ResponseAPI<CardsModel>()
                {
                    Succeeded = false,
                    Message = "Create fail",
                };
            var newCards = _mapper.Map<Card>(request);
            await _dbContext.Cards.AddRangeAsync(newCards);
            await _dbContext.SaveChangesAsync();

            var resul = _mapper.Map<CardsModel>(newCards);
            return new ResponseAPI<CardsModel>()
            {
                Succeeded = true,
                Message = "Create Success",
                Data = resul,
            };
        }

        public async Task<ResponseAPI> DeleteAsync(string id)
        {
            var cards =  _dbContext.Cards.FirstOrDefault(c => c.Id == id);
            if(cards is null)
                return new ResponseAPI()
                {
                    Succeeded = false,
                    Message = "Delete fail cards not found",
                };
            _dbContext.Cards.Remove(cards);
            await _dbContext.SaveChangesAsync();
            return new ResponseAPI()
            {
                Succeeded = true,
                Message = "Delete Success",
            };
        }

        public async Task<ResponseAPI<CardsModel>> GetAsyn(string id)
        {
            var result = await _dbContext.Cards.ProjectTo<CardsModel>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(c => c.Id == id);
            if(result is null)
                return new ResponseAPI<CardsModel>()
                {
                    Succeeded = false,
                    Message = "Get Fail",
                };
            return new ResponseAPI<CardsModel>()
            {
                Succeeded = true,
                Message = "Get Success",
                Data = result
            };
        }

        public async Task<ResponseAPI<IList<CardsModel>>> GetAllAsync()
        {
            var result = await _dbContext.Cards.ProjectTo<CardsModel>(_mapper.ConfigurationProvider)
                 .ToListAsync();
            return new ResponseAPI<IList<CardsModel>>()
            {
                Succeeded = true,
                Message = "GetAll Success",
                Data = result
            };

        }
    }
}

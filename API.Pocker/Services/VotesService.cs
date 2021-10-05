using API.Pocker.Data;
using API.Pocker.Data.Entities;
using API.Pocker.Models;
using API.Pocker.Models.Votes;
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
    public class VotesService : IVotesService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public VotesService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResponseAPI<VotesModel>> CreateAsync(VotesRequest request)
        {
            if (request is null)
                return new ResponseAPI<VotesModel>()
                {
                    Succeeded = false,
                    Message = "Create fail",
                };
            var user =_dbContext.UserProfiles.FirstOrDefault(c => c.Id == request.UserId);
            var card = _dbContext.Cards.FirstOrDefault(c => c.Id == request.CardId);
            var history = _dbContext.UserProfileHistorys.FirstOrDefault(c => c.Id == request.HistoryId);

            if(user is null || card is null || history is null)
                return new ResponseAPI<VotesModel>()
                {
                    Succeeded = false,
                    Message = "Create fail : user, card or history no found",
                };

            var newvotes = new Votes
            { 
                UserHistoryId = history.Id,
                CardsId = card.Id,
                UserProfileId = user.Id
            };
            await _dbContext.Votes.AddAsync(newvotes);
            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<VotesModel>(newvotes);
            return new ResponseAPI<VotesModel>()
            {
                Succeeded = true,
                Message = "Create Success",
                Data = result,
            };
        }

        public async Task<ResponseAPI> DeleteAsync(string id)
        {
            var votes = _dbContext.Votes.FirstOrDefault(c => c.Id == id);
            if (votes is null)
                return new ResponseAPI()
                {
                    Succeeded = false,
                    Message = "Delete fail wishes not found",
                };
            _dbContext.Votes.Remove(votes);
            await _dbContext.SaveChangesAsync();
            return new ResponseAPI()
            {
                Succeeded = true,
                Message = "Delete Success",
            };
        }

        public async Task<ResponseAPI<IList<VotesModel>>> GetAllAsync()
        {
            var result = await _dbContext.Votes.ProjectTo<VotesModel>(_mapper.ConfigurationProvider)
               .ToListAsync();
            return new ResponseAPI<IList<VotesModel>>()
            {
                Succeeded = true,
                Message = "GetAll Success",
                Data = result
            };
        }

        public async Task<ResponseAPI<VotesModel>> GetAsync(string id)
        {
            var result = await _dbContext.Cards.ProjectTo<VotesModel>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(c => c.Id == id);
            if (result is null)
                return new ResponseAPI<VotesModel>()
                {
                    Succeeded = false,
                    Message = "Get Fail",
                };
            return new ResponseAPI<VotesModel>()
            {
                Succeeded = true,
                Message = "Get Success",
                Data = result
            };
        }
    }
}

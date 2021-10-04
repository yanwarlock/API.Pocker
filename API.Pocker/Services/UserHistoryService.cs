using API.Pocker.Data;
using API.Pocker.Data.Entities;
using API.Pocker.Models;
using API.Pocker.Models.User;
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
    public class UserHistoryService: IUserHistoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public UserHistoryService(ApplicationDbContext dbContext , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResponseAPI<UserHistoryModel>> CreateAsync(UserHistoryRequest request)
        {
            if (request is null)
                return new ResponseAPI<UserHistoryModel>()
                {
                    Succeeded = false,
                    Message = "Create fail",
                };
            var newHistory = _mapper.Map<UserProfileHistory>(request);
            await _dbContext.UserProfileHistorys.AddRangeAsync(newHistory);
            await _dbContext.SaveChangesAsync();

            var resul = _mapper.Map<UserHistoryModel>(newHistory);
            return new ResponseAPI<UserHistoryModel>()
            {
                Succeeded = true,
                Message = "Create Success",
                Data = resul,
            };
        }

        public async Task<ResponseAPI<IList<UserHistoryModel>>> GetAllAsync()
        {
            var result = await _dbContext.UserProfileHistorys.ProjectTo<UserHistoryModel>(_mapper.ConfigurationProvider)
                 .ToListAsync();
            return new ResponseAPI<IList<UserHistoryModel>>()
            {
                Succeeded = true,
                Message = "GetAll Success",
                Data = result
            };
        }

    }
}

using API.Pocker.Data;
using API.Pocker.Data.Entities;
using API.Pocker.Models;
using API.Pocker.Models.ManageAccounts;
using API.Pocker.Models.User;
using API.Pocker.Services.Interfaces;
using API.Pocker.Services.ManageAccounts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Services
{
    public class UserProfileService: IUserProfileService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ManageAccountService _manageAccountService; 
        public UserProfileService(ApplicationDbContext dbContext, IMapper mapper, ManageAccountService manageAccountService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _manageAccountService = manageAccountService;
        }

        public async Task<ResponseAPI<UserProfileModel>> CreateAsync(UserProfileRequest request)
        {
            if (request is null)
                return new ResponseAPI<UserProfileModel>()
                {
                    Succeeded = false,
                    Message = "Create fail",
                };
            var accountData = await _manageAccountService.CreateAsync(request.CreateAccountRequest);
            if(!accountData.Succeeded)
                return new ResponseAPI<UserProfileModel>()
                {
                    Succeeded = false,
                    Message = accountData.Message,
                };
            var userProfile = new UserProfile
            {
                Name = request.Name,
                IdUserIdentity = accountData.Data.Id,

            };

            await _dbContext.UserProfiles.AddAsync(userProfile);
            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<UserProfileModel>(userProfile);
            result.Account = accountData.Data;
            return new ResponseAPI<UserProfileModel>()
            {
                Succeeded = true,
                Message = "Create Success",
                Data = result,
            };
        }

        public async Task<ResponseAPI> DeleteAsync(string id)
        {
            var user = _dbContext.UserProfiles.FirstOrDefault(c => c.Id == id);
            if (user is null)
                return new ResponseAPI()
                {
                    Succeeded = false,
                    Message = "Delete fail user not found",
                };
            _dbContext.UserProfiles.Remove(user);
            await _dbContext.SaveChangesAsync();
            return new ResponseAPI()
            {
                Succeeded = true,
                Message = "Delete Success",
            };
        }

        public async Task<ResponseAPI<IList<UserProfileModel>>> GetAllAsync()
        {
            var userProfiles = await _dbContext.UserProfiles.ToListAsync();
            var result = new List<UserProfileModel>();
            foreach (var profile in userProfiles)
            {
                var account = await _dbContext.Users.FirstOrDefaultAsync(a => a.Id == profile.IdUserIdentity);
                var rols = await _manageAccountService.GetRolsAccountAsync(account.Id).ConfigureAwait(false);
                result.Add(new UserProfileModel
                {
                    Id = profile.Id,
                    Name = profile.Name,
                    Account = new AccountModel
                    {
                        Email = account.Email,
                        Id = account.Id,
                        Role = rols.Data,
                    },
                });
            }
            return new ResponseAPI<IList<UserProfileModel>>()
            {
                Succeeded = true,
                Message = "GetAll Success",
                Data = result
            };
        }

        public async Task<ResponseAPI<UserProfileModel>> GetAsync(string request)
        {
            var result = await _dbContext.UserProfiles.ProjectTo<UserProfileModel>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(c => c.Id == request);
            if (result is null)
                return new ResponseAPI<UserProfileModel>()
                {
                    Succeeded = false,
                    Message = "Get Fail",
                };
            return new ResponseAPI<UserProfileModel>()
            {
                Succeeded = true,
                Message = "Get Success",
                Data = result
            };
        }
    }
}

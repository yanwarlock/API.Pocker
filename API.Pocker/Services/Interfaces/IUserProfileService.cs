using API.Pocker.Data.Entities;
using API.Pocker.Models;
using API.Pocker.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<ResponseAPI<UserProfileModel>> CreateAsync(UserProfileRequest request);
        Task<ResponseAPI> DeleteAsync(string id);
        Task<ResponseAPI<IList<UserProfileModel>>> GetAllAsync();
        Task<ResponseAPI<UserProfileModel>> GetAsync(string id);
    }
}

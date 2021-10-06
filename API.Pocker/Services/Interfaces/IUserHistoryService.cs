using API.Pocker.Data.Entities;
using API.Pocker.Models;
using API.Pocker.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Services.Interfaces
{
   public interface IUserHistoryService
    {
        Task<ResponseAPI<UserHistoryModel>> CreateAsync(UserHistoryRequest request);
        Task<ResponseAPI<IList<UserHistoryModel>>> GetAllAsync();
        Task<ResponseAPI<UserHistoryModel>> GetAsyn(string id);
    }
}

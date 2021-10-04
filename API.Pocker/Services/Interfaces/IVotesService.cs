using API.Pocker.Data.Entities;
using API.Pocker.Models;
using API.Pocker.Models.Votes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Services.Interfaces
{
    public interface IVotesService
    {
        Task<ResponseAPI<VotesModel>> CreateAsync(VotesRequest request);
        Task<ResponseAPI> DeleteAsync(string id);
        Task<ResponseAPI<IList<VotesModel>>> GetAllAsync();
    }
}

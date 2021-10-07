using API.Pocker.Models;
using API.Pocker.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Pocker.Services.Interfaces
{
   public interface ICardsService
    {
        Task<ResponseAPI<CardsModel>> CreateAsync(CardsRequest request);
        Task<ResponseAPI> DeleteAsync(string id);
        Task<ResponseAPI<IList<CardsModel>>> GetAllAsync();
        Task<ResponseAPI<CardsModel>> GetAsync(string id);

    }
}

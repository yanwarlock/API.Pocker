using API.Pocker.Models;
using API.Pocker.Models.ManageAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Services.ManageAccounts
{
   public interface IManageAccountService
    {
        Task<ResponseAPI<AccountModel>> CreateAsync(CreateAccountRequest model);
        Task<ResponseAPI<AuthenticationModel>> AuthenticateAsync(AuthenticationRequest model);
        Task<ResponseAPI<AccountModel>> GetAccountAsync(string id);
        Task<ResponseAPI<RefreshTokenModel>> RefreshToken(string model);
        Task<ResponseAPI<IList<string>>> GetRolsAccountAsync(string model);
    }
}

using API.Pocker.Data;
using API.Pocker.Models;
using API.Pocker.Models.ManageAccounts;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Pocker.Settings;
using Microsoft.Extensions.Options;
using API.Pocker.Helpers;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using API.Pocker.Data.Entities.ManagerUser;
using Microsoft.Extensions.Configuration;

namespace API.Pocker.Services.ManageAccounts
{
    public class ManageAccountService : IManageAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ManageAccountService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IConfiguration configuration,
            IOptions<JwtSettings> optionsJwt,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _configuration = configuration;
            _jwtSettings = optionsJwt.Value;
        }
 
        public async Task<ResponseAPI<AuthenticationModel>> AuthenticateAsync(AuthenticationRequest request)
        {
            var erroApp = new IdentityErrorDescriber();
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null)
                return new ResponseAPI<AuthenticationModel>
                {
                    Succeeded = false,
                    Message = $"User is incorrect",
                    Errors = erroApp.InvalidUserName(request.UserName)
                };
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
                return new ResponseAPI<AuthenticationModel>
                {
                    Succeeded = false,
                    Message = $"Wrong credentials for the user",
                    Errors = erroApp.PasswordMismatch()
                };

            var jwtSecurityToken = await GenerateJWToken(user).ConfigureAwait(false);
            var response = new AuthenticationModel
            {
                Id = user.Id,
                JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                Created = DateTime.UtcNow,
            };

            var rolList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var accountNewRefreshToken = await _applicationDbContext.RefreshToken
                .Where(t => t.AccountId == user.Id).FirstOrDefaultAsync();

            var generateRefreshToken = new RefreshTokenModel();
            if (accountNewRefreshToken is null)
            {
                 generateRefreshToken = GenerateRefreshToken();
                await _applicationDbContext.RefreshToken.AddAsync(new RefreshToken
                {
                    AccountId = user.Id,
                    Value = generateRefreshToken.Token,
                });
                await _applicationDbContext.SaveChangesAsync();
                response.RefreshToken = generateRefreshToken;
                response.Role = rolList.ToList();
                return new ResponseAPI<AuthenticationModel>(response)
                {
                    Succeeded = true,
                    Message = "Authentication Success",
                };
            }
            response.Role = rolList.ToList();
            response.RefreshToken = new RefreshTokenModel
            { 
                Token = accountNewRefreshToken.Value,
            };
           
            return new ResponseAPI<AuthenticationModel>(response)
            {
                Succeeded = true,
                Message = "Authentication Success",
            };
        }

        private async Task<JwtSecurityToken> GenerateJWToken(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id)
                }
                .Union(userClaims)
                .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private static RefreshTokenModel GenerateRefreshToken()
        {
            var token = RandomTokenString();
            var resul = new RefreshTokenModel
            {
                Token = token,
            };

            return resul;
        }
        private static string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public async Task<ResponseAPI<AccountModel>> CreateAsync(CreateAccountRequest request)
        {
            var erroApp = new IdentityErrorDescriber();

            var u = await _userManager.FindByEmailAsync(request.Email);
            if (u != null)
                return new ResponseAPI<AccountModel>()
                {
                    Succeeded = false,
                    Message = $"Email '{request.Email}' is already registered",
                    Errors = erroApp.DuplicateEmail(request.Email).Description,
                };

            var identityUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.UserName
            };
            var createIdentityUser = await _userManager.CreateAsync(identityUser, request.Password);

            if (!createIdentityUser.Succeeded)
                return new ResponseAPI<AccountModel>
                {
                    Succeeded = false,
                    Message = $"Unable to create the user '{request.UserName}'",
                };
            var role = _configuration.GetValue<string>("DefaultRol");

            var addToAndoRole = await _userManager.AddToRoleAsync(identityUser, role);
            if (!addToAndoRole.Succeeded)
            {
                await _userManager.DeleteAsync(identityUser);
                return new ResponseAPI<AccountModel>
                {
                    Succeeded = false,
                    Message = $"Unable to assign a role to the user:{request.UserName}",
                };
            }
            var result = _mapper.Map<AccountModel>(identityUser);
            result.Role.Add(role);
            return new ResponseAPI<AccountModel>(result)
            {
                Succeeded = true,
                Message = "Created User"
            };
        }

        public Task<ResponseAPI<AccountModel>> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseAPI<AccountModel>> GetAccountAsync(string id)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user is null)
                return new ResponseAPI<AccountModel>()
                {
                    Succeeded = false,
                    Message = "GetAccount fail account not found"
                };
            var result = _mapper.Map<AccountModel>(user);
            result.Role = await _userManager.GetRolesAsync(user);

            return new ResponseAPI<AccountModel>(result)
            {
                Succeeded = true,
                Message = "GetAccount Success"
            };
        }

        public Task<ResponseAPI<IList<AccountModel>>> GetAllAccountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseAPI<IList<string>>> GetRolsAccountAsync(string id)
        {
            var result = await _userManager.GetRolesAsync(await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == id));
            return new ResponseAPI<IList<string>>()
            {
                Succeeded = true,
                Message = "GetRolsAccount Success",
                Data = result,
                
            };
        }

        public async Task<ResponseAPI<RefreshTokenModel>> RefreshToken(string request)
        {
            var erroApp = new IdentityErrorDescriber();
            var idUserToken = await _applicationDbContext.RefreshToken.Where(t => t.Value == request).FirstOrDefaultAsync();
            if (idUserToken is null)
            {
                return new ResponseAPI<RefreshTokenModel>()
                {
                    Succeeded = false,
                    Errors = erroApp.InvalidToken()
                };
            }
            var user = await _userManager.FindByIdAsync(idUserToken.AccountId);
            var jwtSecurityToken = await GenerateJWToken(user).ConfigureAwait(false);
            var result = new RefreshTokenModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };
            return new ResponseAPI<RefreshTokenModel>(result)
            {
                Succeeded = true,
                Message = "RefreshToken Success",
            };
        }
        public Task<ResponseAPI<IList<string>>> GetRols(string id)
        {
            throw new NotImplementedException();
        }
    }
}

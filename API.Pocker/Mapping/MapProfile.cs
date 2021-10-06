using API.Pocker.Data.Entities;
using API.Pocker.Data.Entities.ManagerUser;
using API.Pocker.Models.Cards;
using API.Pocker.Models.ManageAccounts;
using API.Pocker.Models.User;
using API.Pocker.Models.Votes;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Pocker.Mapping
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<CardsRequest, Card>()
                .ReverseMap();
            CreateMap<CardsModel, Card>()
                .ReverseMap();

            CreateMap<UserHistoryRequest, UserProfileHistory>()
                .ReverseMap();

            CreateMap<UserHistoryModel, UserProfileHistory>()
                    .ReverseMap();
            CreateMap<UserProfileHistory, UserHistoryModel>()
                .ForMember(x => x.UserName, src => src.MapFrom(x => x.UserProfile.Name));
            CreateMap<UserHistoryModel, UserProfileHistory>()
               .ForMember(x => x.UserProfile, src => src.MapFrom(x => x.UserName));


            CreateMap<UserProfileRequest, UserProfile>()
                .ReverseMap();

            CreateMap<UserProfileModel, UserProfile>()
                .ReverseMap();
            CreateMap<UserProfile, UserProfileModel>()
                .ForMember(x => x.Account, src => src.MapFrom(x => x.UserIdentity));

            CreateMap<VotesRequest, Votes>()
                .ReverseMap();
            CreateMap<VotesModel, Votes>()
                .ReverseMap();
            CreateMap<Votes, VotesModel>()
                .ForMember(x => x.UserProfile, src => src.MapFrom(x => x.UserProfile));

            CreateMap<CreateAccountRequest, IdentityUser>()
               .ReverseMap();
            CreateMap<AccountModel, IdentityUser>()
                .ReverseMap();
            CreateMap<RefreshTokenModel, RefreshToken>()
                .ReverseMap();

        }
    }
}

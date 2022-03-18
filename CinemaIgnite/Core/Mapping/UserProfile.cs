using AutoMapper;
using Core.ViewModels.User;
using Infrastructure.Models;

namespace Core.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserModel, User>();

            CreateMap<User, UserProfileModel>();
        }
    }
}

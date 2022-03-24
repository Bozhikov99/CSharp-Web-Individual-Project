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

            CreateMap<User, EditUserModel>()
                .ReverseMap();

            CreateMap<User, UserListModel>()
                .ForMember(d => d.FullName, s => s.MapFrom(u => $"{u.FirstName} {u.LastName}"));
        }
    }
}

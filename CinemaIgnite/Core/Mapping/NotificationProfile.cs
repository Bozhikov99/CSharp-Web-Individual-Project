using AutoMapper;
using Core.ViewModels.Notification;
using Infrastructure.Models;

namespace Core.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDetailsModel>();
        }
    }
}

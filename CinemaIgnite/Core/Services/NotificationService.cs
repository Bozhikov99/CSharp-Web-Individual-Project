using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Services.Contracts;
using Core.ViewModels.Notification;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public NotificationService(IMapper mapper, IRepository repository)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<int> GetUnreadCount(string userId)
        {
            IEnumerable<Notification> unreadNotifications = await repository.All<Notification>(n => n.UserId == userId && !n.IsChecked)
                .ToArrayAsync();

            return unreadNotifications.Count();
        }

        public async Task Read(string id)
        {
            Notification notification = await repository.GetByIdAsync<Notification>(id);
            notification.IsChecked = true;

            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationDetailsModel>> GetAll(string userId)
        {
            IEnumerable<NotificationDetailsModel> notifications = await repository.All<Notification>(n => n.UserId == userId)
                .ProjectTo<NotificationDetailsModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return notifications;
        }

        public async Task Delete(string id)
        {
            await repository.DeleteAsync<Notification>(id);
            await repository.SaveChangesAsync();
        }
    }
}

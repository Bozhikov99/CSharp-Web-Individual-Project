using Core.Services.Contracts;
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

        public NotificationService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> GetUnreadCount(string userId)
        {
            IEnumerable<Notification> unreadNotifications = await repository.All<Notification>(n => n.UserId == userId)
                .ToArrayAsync();

            return unreadNotifications.Count();
        }
    }
}

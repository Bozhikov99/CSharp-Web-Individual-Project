using Core.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public interface INotificationService
    {
        Task<int> GetUnreadCount(string userId);

        Task Read(string id);

        Task<IEnumerable<NotificationDetailsModel>> GetAll(string userId);

        Task Delete(string[] ids);
    }
}

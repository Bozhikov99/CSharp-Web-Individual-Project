using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public interface INotificationService
    {
        //Task<T> GetAll(string userId)                         - To list
        //Task<T> GetById(string id)                            - To read
        //Task<bool> Delete(string id)                            - To delete
        Task<int> GetUnreadCount(string userId);

        Task Read(string id);
    }
}

using Infrastructure.Common;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : Repository, INotificationRepository
    {
        public NotificationRepository(CinemaDbContext context)
        {
            Context = context;
        }
    }
}

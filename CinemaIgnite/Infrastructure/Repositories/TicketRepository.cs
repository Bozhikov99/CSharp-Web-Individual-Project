using Infrastructure.Common;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class TicketRepository : Repository, ITicketRepository
    {
        public TicketRepository(CinemaDbContext context)
        {
            Context = context;
        }
    }
}

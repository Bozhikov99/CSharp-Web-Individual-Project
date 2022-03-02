using Infrastructure.Common;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(CinemaDbContext context)
        {
            Context = context;
        }
    }
}

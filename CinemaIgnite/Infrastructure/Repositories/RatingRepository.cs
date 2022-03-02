using Infrastructure.Common;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class RatingRepository : Repository, IRatingRepository
    {
        public RatingRepository(CinemaDbContext context)
        {
            Context = context;
        }
    }
}

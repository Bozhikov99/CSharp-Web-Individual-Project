using Infrastructure.Common;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class MovieRepository : Repository, IMovieRepository
    {
        public MovieRepository(CinemaDbContext context)
        {
            Context = context;
        }
    }
}

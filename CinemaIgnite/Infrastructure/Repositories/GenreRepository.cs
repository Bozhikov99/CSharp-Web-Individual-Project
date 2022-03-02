using Infrastructure.Common;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class GenreRepository : Repository, IGenreRepository
    {
        public GenreRepository(CinemaDbContext context)
        {
            Context = context;
        }
    }
}

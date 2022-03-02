using Infrastructure.Common;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class ProjectionRepository: Repository, IProjectionRepository
    {
        public ProjectionRepository(CinemaDbContext context)
        {
            Context = context;
        }
    }
}

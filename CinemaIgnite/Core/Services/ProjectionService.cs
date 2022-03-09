using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.ViewModels.Projection;
using Infrastructure.Contracts;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public class ProjectionService : IProjectionService
    {
        private readonly IProjectionRepository repository;

        private readonly IMapper mapper;

        public ProjectionService(IMapper mapper, IProjectionRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }
        public Task<(bool isCreated, string error)> Create(CreateProjectionModel model, string movieId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                await repository.DeleteAsync<Projection>(id);
                await repository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAllForMovie(string movieId)
        {
            try
            {
                repository.DeleteRange<Projection>(p => p.MovieId == movieId);
                await repository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ListProjectionModel>> GetAllForDate(DateTime date)
        {
            IEnumerable<ListProjectionModel> projections = await repository.All<Projection>()
                .Where(p => p.Date == date)
                .ProjectTo<ListProjectionModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return projections;
        }

        public async Task<IEnumerable<ListProjectionModel>> GetAllForMovie(string movieId)
        {
            IEnumerable<ListProjectionModel> projections = await repository.All<Projection>()
                .Where(p => p.MovieId == movieId)
                .ProjectTo<ListProjectionModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return projections;
        }

        public async Task<ProjectionDetails> GetProjectionById(string id)
        {
            Projection projection = await GetById(id);
            ProjectionDetails details = mapper.Map<ProjectionDetails>(projection);

            return details;
        }

        public async Task<ProjectionDetails> GetProjectionDetails(string id)
        {
            Projection projection = await GetById(id);
            ProjectionDetails details = mapper.Map<ProjectionDetails>(projection);

            return details;
        }

        public bool IsPlaceTaken(int seat)
        {
            bool isTaken = repository.AllReadonly<Ticket>()
                .Any(t => t.Seat == seat);

            return isTaken;
        }

        private async Task<Projection> GetById(string id)
        {
            Projection projection = await repository.GetByIdAsync<Projection>(id);

            return projection;
        }
    }
}

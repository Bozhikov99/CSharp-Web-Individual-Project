using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class GenreService : IGenreService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GenreService(IMapper mapper, IRepository repository)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task Create(CreateGenreModel model)
        {
            Genre checkExisting = await repository.All<Genre>(g => g.Name == model.Name)
                .FirstOrDefaultAsync();

            Genre genre = mapper.Map<Genre>(model);

            if (checkExisting != null)
            {
                string error = string.Format(ErrorMessagesConstants.GenreNameException, model.Name);
                throw new ArgumentException(error);
            }

            await repository.AddAsync(genre);
            await repository.SaveChangesAsync();
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                await repository.DeleteAsync<Genre>(id);
                await repository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Edit(EditGenreModel model)
        {
            Genre genre = mapper.Map<Genre>(model);

            try
            {
                repository.Update(genre);
                await repository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ICollection<ListGenreModel>> GetAll()
        {
            ICollection<ListGenreModel> genres = await repository.AllReadonly<Genre>()
                .ProjectTo<ListGenreModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return genres;
        }

        public async Task<ListGenreModel> GetById(string id)
        {
            Genre genre = await repository.GetByIdAsync<Genre>(id);
            ListGenreModel model = mapper.Map<ListGenreModel>(genre);

            return model;
        }
    }
}

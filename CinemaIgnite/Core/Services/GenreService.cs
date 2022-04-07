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
            bool exists = await Exists(model.Name);

            if (exists)
            {
                string error = string.Format(ErrorMessagesConstants.GenreNameException, model.Name);
                throw new ArgumentException(error);
            }

            Genre genre = mapper.Map<Genre>(model);

            await repository.AddAsync(genre);
            await repository.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            Genre genre = await repository.GetByIdAsync<Genre>(id);

            if (genre == null)
            {
                throw new ArgumentException(ErrorMessagesConstants.GenreDoesNotExist);
            }

            await repository.DeleteAsync<Genre>(id);
            await repository.SaveChangesAsync();
        }

        public async Task<bool> Edit(EditGenreModel model)
        {
            bool exists = await Exists(model.Name);

            if (exists)
            {
                throw new ArgumentException(ErrorMessagesConstants.GenreNameException);
            }

            //Genre genre = mapper.Map<Genre>(model);
            Genre genre=await repository.GetByIdAsync<Genre>(model.Id);
            genre.Name = model.Name;

            repository.Update(genre);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<EditGenreModel> GetEditModel(string id)
        {
            Genre movie = await repository.GetByIdAsync<Genre>(id);
            EditGenreModel model = mapper.Map<EditGenreModel>(movie);

            return model;
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

            if (genre == null)
            {
                throw new ArgumentException(ErrorMessagesConstants.GenreDoesNotExist);
            }

            ListGenreModel model = mapper.Map<ListGenreModel>(genre);

            return model;
        }

        private async Task<bool> Exists(string name)
        {
            Genre checkExisting = await repository.All<Genre>(g => g.Name == name)
                .FirstOrDefaultAsync();

            return checkExisting != null;
        }
    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public MovieService(IMapper mapper, IRepository repository)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ListMovieModel>> GetAll()
        {
            IEnumerable<ListMovieModel> movies = await repository.All<Movie>()
                .ProjectTo<ListMovieModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return movies;
        }

        public async Task<IEnumerable<ListMovieModel>> GetAll(Expression<Func<Movie, bool>> search)
        {
            IEnumerable<ListMovieModel> movies = await repository.All(search)
                .ProjectTo<ListMovieModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return movies;
        }

        public async Task Create(CreateMovieModel model)
        {
            bool exists = await Exists(model.Title);

            if (exists)
            {
                throw new ArgumentException(ErrorMessagesConstants.MovieNameException);
            }

            Movie movie = mapper.Map<Movie>(model);

            foreach (string id in model.GenreIds)
            {
                Genre currentGenre = await repository.GetByIdAsync<Genre>(id);

                movie.Genres
                    .Add(currentGenre);
            }

            await repository.AddAsync(movie);
            await repository.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            Movie movie = await repository.GetByIdAsync<Movie>(id);

            if (movie == null)
            {
                throw new ArgumentException(ErrorMessagesConstants.MovieDoesNotExist);
            }

            await repository.DeleteAsync<Movie>(id);
            await repository.SaveChangesAsync();

        }

        public async Task<EditMovieModel> GetEditModel(string id)
        {
            Movie movie = await repository.GetByIdAsync<Movie>(id);
            EditMovieModel model = mapper.Map<EditMovieModel>(movie);

            return model;
        }

        public async Task Edit(EditMovieModel model)
        {

            var movie = repository.All<Movie>()
                .Include(m => m.Genres)
                .First(m => m.Id == model.Id);

            movie.Genres.Clear();

            List<Genre> genres = new List<Genre>();

            foreach (string id in model.GenreIds)
            {
                Genre currentGenre = await repository.GetByIdAsync<Genre>(id);
                genres.Add(currentGenre);
            }

            movie.Genres = genres;
            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.Director = model.Director;
            movie.Country = model.Country;
            movie.Actors = model.Actors;
            movie.Duration = model.Duration;
            movie.ImageUrl = model.ImageUrl;
            movie.ReleaseYear = model.ReleaseYear;

            await repository.SaveChangesAsync();
        }


        public async Task<MovieDetailsModel> GetMovieDetails(string id)
        {
            Movie movie = repository.All<Movie>(m => m.Id == id)
                .Include(m => m.Ratings)
                .Include(m => m.Genres)
                .First();

            MovieDetailsModel model = mapper.Map<MovieDetailsModel>(movie);

            string rating = $"{model.Rating:F2}";

            return model;
        }
        public async Task<string> GetRating(string id)
        {
            Movie movie = repository.All<Movie>(m => m.Id == id)
               .Include(m => m.Ratings)
               .First();

            MovieDetailsModel model = mapper.Map<MovieDetailsModel>(movie);
            string rating = $"{model.Rating:F1}";

            return rating;
        }

        private async Task<bool> Exists(string name)
        {
            Movie movie = await repository.All<Movie>(m => m.Title == name)
                .FirstOrDefaultAsync();

            return movie != null;
        }
    }
}

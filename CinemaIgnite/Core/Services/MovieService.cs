using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public async Task<bool> Create(CreateMovieModel model)
        {
            Movie movie = mapper.Map<Movie>(model);

            foreach (string id in model.GenreIds)
            {
                Genre currentGenre = await repository.GetByIdAsync<Genre>(id);

                movie.Genres
                    .Add(currentGenre);
            }

            try
            {
                await repository.AddAsync(movie);
                await repository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                await repository.DeleteAsync<Movie>(id);
                await repository.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<EditMovieModel> GetEditModel(string id)
        {
            Movie movie = await repository.GetByIdAsync<Movie>(id);
            EditMovieModel model = mapper.Map<EditMovieModel>(movie);

            return model;
        }

        public async Task<bool> Edit(EditMovieModel model)
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

            try
            {
                await repository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<MovieDetailsModel> GetMovieDetails(string id)
        {
            Movie movie = repository.All<Movie>(m => m.Id == id)
                .Include(m => m.Ratings)
                .First();

            MovieDetailsModel model = mapper.Map<MovieDetailsModel>(movie);

            //model.Rating = movie.Ratings
            //    .Select(r => r.Value)
            //    .Sum() / movie.Ratings.Count;

            string rating = $"{model.Rating:F2}";

            return model;
        }
        public async Task<string> GetRating(string id)
        {
            //Movie movie = await repository.GetByIdAsync<Movie>(id);
            //MovieDetailsModel model = mapper.Map<MovieDetailsModel>(movie);
            Movie movie = repository.All<Movie>(m => m.Id == id)
               .Include(m => m.Ratings)
               .First();

            MovieDetailsModel model = mapper.Map<MovieDetailsModel>(movie);
            string rating = $"{model.Rating:F1}";

            return rating;
        }
    }
}

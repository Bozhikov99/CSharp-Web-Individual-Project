using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Infrastructure.Contracts;
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
        private readonly IMovieRepository movieRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IMapper mapper;

        public MovieService(IMapper mapper, IMovieRepository movieRepository, IGenreRepository genreRepository)
        {
            this.movieRepository = movieRepository;
            this.genreRepository = genreRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ListMovieModel>> GetAll()
        {
            IEnumerable<ListMovieModel> movies = await movieRepository.All<Movie>()
                .ProjectTo<ListMovieModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return movies;
        }

        public async Task<IEnumerable<ListMovieModel>> GetAll(Expression<Func<Movie, bool>> search)
        {
            IEnumerable<ListMovieModel> movies = await movieRepository.All(search)
                .ProjectTo<ListMovieModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return movies;
        }

        public async Task<bool> Create(CreateMovieModel model)
        {
            Movie movie = mapper.Map<Movie>(model);

            foreach (string id in model.GenreIds)
            {
                Genre currentGenre = await genreRepository.GetByIdAsync<Genre>(id);

                movie.Genres
                    .Add(currentGenre);
            }

            try
            {
                await movieRepository.AddAsync(movie);
                await movieRepository.SaveChangesAsync();
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
                await movieRepository.DeleteAsync<Movie>(id);
                await movieRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<EditMovieModel> GetEditModel(string id)
        {
            Movie movie = await movieRepository.GetByIdAsync<Movie>(id);
            EditMovieModel model = mapper.Map<EditMovieModel>(movie);

            return model;
        }

        public async Task<bool> Edit(EditMovieModel model)
        {
            var movie = movieRepository.All<Movie>()
                .Include(m => m.Genres)
                .First(m => m.Id == model.Id);

            movie.Genres.Clear();

            List<Genre> genres = new List<Genre>();

            foreach (string id in model.GenreIds)
            {
                Genre currentGenre = await genreRepository.GetByIdAsync<Genre>(id);
                genres.Add(currentGenre);
            }

            movie.Genres = genres;

            try
            {
                await movieRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

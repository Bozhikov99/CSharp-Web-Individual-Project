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
            IEnumerable<ListMovieModel> movies = await movieRepository.AllReadonly<Movie>()
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

        public async Task<bool> Edit(EditMovieModel model)
        {
            Genre genre = mapper.Map<Genre>(model);

            try
            {
                movieRepository.Update(genre);
                await movieRepository.SaveChangesAsync();
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}

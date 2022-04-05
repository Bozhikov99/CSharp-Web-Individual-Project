using AutoMapper;
using Core.Mapping;
using Core.Services;
using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class MovieServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IMovieService service;
        private IMapper mapper;
        private string testGenreId;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddAutoMapper(cfg => cfg.AddProfile<MovieProfile>())
                .AddSingleton<IMovieService, MovieService>()
                .BuildServiceProvider();

            IRepository repository = serviceProvider.GetService<IRepository>();
            service = serviceProvider.GetService<IMovieService>();
            mapper = serviceProvider.GetService<IMapper>();

            await SeedDbAsync(repository);
        }

        [Test]
        public async Task GetAll_ReturnsCorrect_Count()
        {
            int expected = 2;
            ListMovieModel[] movies = await service.GetAll() as ListMovieModel[];

            Assert.AreEqual(expected, movies.Length);
        }

        [Test]
        public async Task Create_ThrowsException_WhenMovieExists()
        {

            CreateMovieModel movie = new CreateMovieModel()
            {
                Title = "Test movie",
                ImageUrl = "Url",
                Description = "Simple description about a test movie, that needs no further explaining",
                ReleaseYear = 2005,
                Actors = "Some Actor, Test",
                Director = "Some Director",
                Country = "Principality of Bulgaria",
                GenreIds = new string[] { testGenreId }
            };

            await service.Create(movie);

            Assert.ThrowsAsync<ArgumentException>(async () => await service.Create(movie));
        }

        [Test]
        public async Task Create_Succesfully()
        {
            int expected = 3;

            CreateMovieModel movie = new CreateMovieModel()
            {
                Title = "Sample movie",
                ImageUrl = "Url",
                Description = "Simple description about a test movie, that needs no further explaining",
                ReleaseYear = 2005,
                Actors = "Some Actor, Test",
                Director = "Some Director",
                Country = "Principality of Bulgaria",
                GenreIds = new string[] { testGenreId }
            };

            await service.Create(movie);
            int actual = (await service.GetAll())
                .Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Delete_Sucessfully()
        {
            int expected = 1;

            ListMovieModel[] moviesInitial = await service.GetAll() as ListMovieModel[];
            ListMovieModel movie = moviesInitial[0];
            string movieId = movie.Id;

            await service.Delete(movieId);

            ListMovieModel[] movies = await service.GetAll() as ListMovieModel[];

            Assert.AreEqual(expected, movies.Count());
        }

        [Test]
        public async Task Delete_ThrowsException_WhenIdIsIncorrect()
        {
            string id = "random";

            Assert.ThrowsAsync<ArgumentException>(async () => await service.Delete(id));
        }

        [TearDown]
        public async Task TearDown()
        {
            await dbContext.Dispose();
        }

        private async Task SeedDbAsync(IRepository repository)
        {
            Genre testGenre = new Genre()
            {
                Name = "Genre"
            };

            Movie firstTestMovie = new Movie()
            {
                Title = "Test",
                ImageUrl = "SomeUrl",
                Description = "Simple description about a test movie",
                Duration = new TimeSpan(1, 45, 2),
                ReleaseYear = 1991,
                Actors = "Arnold Weissneger",
                Director = "Some Director",
                Country = "Austria-Hungary",
                Genres = new List<Genre>() { testGenre }
            };

            Movie secondTestMovie = new Movie()
            {
                Title = "Second Test",
                ImageUrl = "SomeUrl",
                Description = "Simple description about a test movie",
                Duration = new TimeSpan(1, 45, 2),
                ReleaseYear = 1991,
                Actors = "Joshua Kimmich",
                Director = "Some Director",
                Country = "Austria-Hungary",
                Genres = new List<Genre>() { testGenre }
            };

            await repository.AddAsync(testGenre);
            await repository.AddAsync(firstTestMovie);
            await repository.AddAsync(secondTestMovie);
            await repository.SaveChangesAsync();

            testGenreId = repository.All<Genre>().First().Id;
        }
    }
}

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

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
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

        [TearDown]
        public async Task TearDown()
        {
            await dbContext.Dispose();
        }

        private async Task SeedDbAsync(IRepository repository)
        {
            Genre testGenre=new Genre()
            {
                Name = "Genre"
            };

            Movie firstTestMovie=new Movie()
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

            await repository.AddAsync(testGenre);
            await repository.AddAsync(firstTestMovie);
            await repository.AddAsync(secondTestMovie);
            await repository.SaveChangesAsync();
        }
    }
}

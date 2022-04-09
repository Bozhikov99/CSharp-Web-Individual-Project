using AutoMapper;
using Common;
using Core.Mapping;
using Core.Services.Contracts;
using Core.ViewModels.Projection;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Tests
{
    public class ProjectionServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IProjectionService service;
        private IMapper mapper;
        private string firstId;
        private string movieId;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddAutoMapper(cfg => cfg.AddProfile<ProjectionProfile>())
                .AddSingleton<IProjectionService, ProjectionService>()
                .BuildServiceProvider();

            IRepository repository = serviceProvider.GetService<IRepository>();
            service = serviceProvider.GetService<IProjectionService>();
            mapper = serviceProvider.GetService<IMapper>();

            await SeedDbAsync(repository);
        }

        [Test]
        public async Task GetAllForDate_ReturnsCorrectCount()
        {
            int expected = 2;
            DateTime date = new DateTime(2022, 5, 15, 15, 30, 0);

            ListProjectionModel[] projections = await service.GetAllForDate(date) as ListProjectionModel[];

            Assert.AreEqual(expected, projections.Length);
        }

        [Test]
        public async Task Create_AddsProjection()
        {
            int expected = 1;

            DateTime date = new DateTime(2022, 1, 15, 10, 15, 0);

            CreateProjectionModel model = new CreateProjectionModel()
            {
                Date = new DateTime(2022, 1, 15, 10, 15, 0),
                TicketsAvailable = ProjectionConstants.TicketsAvailable,
                Subtitles = false,
                Sound = "Bulgarian",
                Price = 2.40m,
                Format = "2D",
                MovieId = movieId
            };

            await service.Create(model);


            IRepository repository = serviceProvider.GetService<IRepository>();

            IEnumerable<Projection> projections = repository.All<Projection>(p => p.Date == date);

            Assert.AreEqual(expected, projections.Count());
        }

        [Test]
        public async Task Delete_RemovesSuccessfully()
        {
            int expected = 1;
            DateTime date = new DateTime(2022, 5, 15, 15, 30, 0);


            await service.Delete(firstId);

            IRepository repository = serviceProvider.GetService<IRepository>();
            IEnumerable<Projection> projections = repository.All<Projection>(p => p.Date == date);

            Assert.AreEqual(expected, projections.Count());
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

            Movie testMovie = new Movie()
            {
                Title = "Test Movie",
                ImageUrl = "SomeUrl",
                Description = "Simple description about a test movie",
                Duration = new TimeSpan(1, 45, 2),
                ReleaseYear = 2015,
                Actors = "Arnold Weissneger",
                Director = "Prajat Kumar",
                Country = "India",
                Genres = new List<Genre>() { testGenre }
            };

            Projection firstProjection = new Projection()
            {
                Date = new DateTime(2022, 5, 15, 15, 30, 0),
                TicketsAvailable = ProjectionConstants.TicketsAvailable,
                Subtitles = false,
                Sound = "Bulgarian",
                Price = 2.40m,
                Format = "2D",
                Movie = testMovie
            };

            Projection secondProjection = new Projection()
            {
                Date = new DateTime(2022, 5, 15, 15, 30, 0),
                TicketsAvailable = ProjectionConstants.TicketsAvailable,
                Subtitles = false,
                Sound = "Bulgarian",
                Price = 3m,
                Format = "3D",
                Movie = testMovie
            };

            User firstUser = new User()
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@abv.bg",
                FavouriteMovies = { }
            };

            await repository.AddAsync(testGenre);
            await repository.AddAsync(testMovie);
            await repository.AddAsync(testMovie);
            await repository.AddAsync(firstProjection);
            await repository.AddAsync(secondProjection);
            await repository.SaveChangesAsync();

            Projection firstFromDb = repository.All<Projection>(p => p.Price == 2.40m)
                .First();

            Projection secondFromDb = repository.All<Projection>(p => p.Price == 3m)
                .First();

            Movie movieFromDb = repository.All<Movie>()
                .First();

            Genre genreFromDb = repository.All<Genre>()
                .First();

            firstId = firstFromDb.Id;
            movieId = movieFromDb.Id;
        }
    }
}

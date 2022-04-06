using AutoMapper;
using Core.Mapping;
using Core.Services;
using Core.Services.Contracts;
using Core.ViewModels.Genre;
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
    public class GenreServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IGenreService service;
        private IMapper mapper;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddAutoMapper(cfg => cfg.AddProfile<GenreProfile>())
                .AddSingleton<IGenreService, GenreService>()
                .BuildServiceProvider();

            IRepository repository = serviceProvider.GetService<IRepository>();
            service = serviceProvider.GetService<IGenreService>();
            mapper = serviceProvider.GetService<IMapper>();

            await SeedDbAsync(repository);
        }

        [Test]
        public async Task GetAll_ReturnsCorrectly()
        {
            string name = "Test genre2";


            ListGenreModel[] genresFromDb = await service.GetAll() as ListGenreModel[];
            ListGenreModel dbModel = genresFromDb.First(g=>g.Name=="Test genre2");

            Assert.AreEqual(dbModel.Name, name);
        }

        [Test]
        public async Task Create_ThrowsException_WhenGenreExists()
        {
            CreateGenreModel genre = new CreateGenreModel()
            {
                Name = "Test genre"
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await service.Create(genre));
        }

        [Test]
        public async Task Create_AddsGenre()
        {
            int count = 3;

            CreateGenreModel genre = new CreateGenreModel()
            {
                Name = "Comedy"
            };

            await service.Create(genre);
            IEnumerable<ListGenreModel> genres = await service.GetAll();

            Assert.AreEqual(count, genres.Count());
        }

        [Test]
        public async Task Delete_ThrowsException_WhenIncorrectId()
        {
            string testId = "testId";

            Assert.ThrowsAsync<ArgumentException>(async () => await service.Delete(testId));
        }

        [Test]
        public async Task Delete_RemovesSuccesfully()
        {
            int expectedCount = 1;

            ListGenreModel[] initialGenres = await service.GetAll() as ListGenreModel[];
            string testId = initialGenres[0].Id;

            await service.Delete(testId);
            ListGenreModel[] expectedGenres = await service.GetAll() as ListGenreModel[];

            Assert.AreEqual(expectedCount, expectedGenres.Count());
        }

        [Test]
        public async Task GetById_ReturnsSuccesfully()
        {
            ListGenreModel[] dbGenres = await service.GetAll() as ListGenreModel[];
            ListGenreModel expectedGenre = dbGenres[0];
            string id = expectedGenre.Id;

            ListGenreModel actualGenre = await service.GetById(id);

            Assert.AreEqual(expectedGenre.Name, actualGenre.Name);
        }

        [Test]
        public async Task GetById_Throws_WhenNotFound()
        {
            string testId = "testId";

            Assert.ThrowsAsync<ArgumentException>(async () => await service.GetById(testId));
        }

        [Test]
        public async Task Edit_Throws_WhenGenreExists()
        {
            string name = "Test genre";
            ListGenreModel[] genres = await service.GetAll() as ListGenreModel[];
            ListGenreModel genre = genres[1];
            EditGenreModel editModel = new EditGenreModel()
            {
                Id = genre.Id,
                Name = name
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await service.Edit(editModel));
        }

        [TearDown]
        public async Task TearDown()
        {
            await dbContext.Dispose();
        }

        private async Task SeedDbAsync(IRepository repository)
        {
            Genre genre = new Genre()
            {
                Name = "Test genre"
            };

            Genre secondGenre = new Genre()
            {
                Name = "Test genre2"
            };

            //Movie movie = new Movie()
            //{
            //    Title = "Test",
            //    ImageUrl = "SomeUrl",
            //    Description = "Simple description about a test movie",
            //    Duration = new TimeSpan(1, 45, 2),
            //    ReleaseYear = 1991,
            //    Actors = "Arnold Weissneger",
            //    Director = "Some Director",
            //    Country = "Austria-Hungary",
            //    Genres = new List<Genre>() { genre }
            //};

            await repository.AddAsync(genre);
            await repository.AddAsync(secondGenre);
            //await repository.AddAsync(movie);
            await repository.SaveChangesAsync();
        }
    }
}

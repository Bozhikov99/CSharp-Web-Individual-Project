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
        private string genreId;

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
            ListGenreModel dbModel = genresFromDb.First(g => g.Name == "Test genre2");

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

            IRepository repository = serviceProvider.GetService<IRepository>();
            IEnumerable<Genre> genres = repository.All<Genre>();

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

            IRepository repository = serviceProvider.GetService<IRepository>();
            IEnumerable<Genre> initialGenres = repository.All<Genre>();
            string testId = initialGenres
                .First().Id;

            await service.Delete(testId);

            IEnumerable<Genre> expectedGenres = repository.All<Genre>();

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

        [Test]
        public async Task Edit_Successfully()
        {
            string name = "New name";
            IRepository repository = serviceProvider.GetService<IRepository>();

            EditGenreModel model = await service.GetEditModel(genreId);
            model.Name = name;

            await service.Edit(model);

            Genre genre = await repository.GetByIdAsync<Genre>(genreId);

            Assert.AreEqual(name, genre.Name);
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

            await repository.AddAsync(genre);
            await repository.AddAsync(secondGenre);
            await repository.SaveChangesAsync();

            Genre firstFromDb = repository.All<Genre>(g => g.Name == "Test genre")
                .First();

            genreId = firstFromDb.Id;
        }
    }
}

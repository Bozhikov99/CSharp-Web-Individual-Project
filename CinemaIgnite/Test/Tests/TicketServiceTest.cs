using AutoMapper;
using Common;
using Core.Mapping;
using Core.Services;
using Core.Services.Contracts;
using Core.ViewModels.Movie;
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
    public class TicketServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private ITicketService service;
        private IMapper mapper;
        private string projectionId;
        private string userId;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddAutoMapper(cfg => cfg.AddProfile<TicketProfile>())
                .AddAutoMapper(cfg => cfg.AddProfile<ProjectionProfile>())
                .AddAutoMapper(cfg => cfg.AddProfile<MovieProfile>())
                .AddSingleton<ITicketService, TicketService>()
                .BuildServiceProvider();

            IRepository repository = serviceProvider.GetService<IRepository>();
            service = serviceProvider.GetService<ITicketService>();
            mapper = serviceProvider.GetService<IMapper>();

            await SeedDbAsync(repository);
        }

        [Test]
        public async Task BuyTickets_AddsTicketCorectly()
        {
            int expected = 3;
            IRepository repository = serviceProvider.GetService<IRepository>();

            int[] testSeats = { 1, 2 };

            await service.BuyTickets(testSeats, projectionId, userId);

            int actual = repository.All<Ticket>()
                .Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetTakenSeats_ReturnsCorrectly()
        {
            int[] expected = { 5 };
            int[] actual = await service.GetTakenSeats(projectionId) as int[];

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetInfo_ReturnsCorrectly()
        {
            //ListProjectionModel
            DateTime date = new DateTime(2022, 5, 15, 15, 30, 0);
            int ticketsAvailable = ProjectionConstants.TicketsAvailable;
            bool subtitles = false;
            string sound = "Bulgarian";
            decimal price = 2.40m;
            string format = "3D";

            string title = "Test Movie";
            string imageUrl = "SomeUrl";
            TimeSpan duration = new TimeSpan(1, 45, 2);
            int releaseYear = 2015;
            double rating = 0;
            string[] genres = { "Genre" };

            (ListMovieModel actualMovie, ListProjectionModel actualProjection) = await service.GetInfo(projectionId);

            Assert.AreEqual(date, actualProjection.Date);
            Assert.AreEqual(ticketsAvailable, actualProjection.TicketsAvailable);
            Assert.AreEqual(subtitles, actualProjection.Subtitles);
            Assert.AreEqual(price, actualProjection.Price);
            Assert.AreEqual(sound, actualProjection.Sound);
            Assert.AreEqual(format, actualProjection.Format);

            Assert.AreEqual(title, actualMovie.Title);
            Assert.AreEqual(imageUrl, actualMovie.ImageUrl);
            Assert.AreEqual(duration, actualMovie.Duration);
            Assert.AreEqual(releaseYear, actualMovie.ReleaseYear);
            Assert.AreEqual(rating, actualMovie.Rating);
            Assert.AreEqual(genres, actualMovie.Genres);
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
                Name = "Genre"
            };

            Movie movie = new Movie()
            {
                Title = "Test Movie",
                ImageUrl = "SomeUrl",
                Description = "Simple description about a test movie",
                Duration = new TimeSpan(1, 45, 2),
                ReleaseYear = 2015,
                Actors = "Arnold Weissneger",
                Director = "Prajat Kumar",
                Country = "India",
                Genres = new List<Genre>() { genre }
            };

            Projection projection = new Projection()
            {
                Date = new DateTime(2022, 5, 15, 15, 30, 0),
                TicketsAvailable = ProjectionConstants.TicketsAvailable,
                Subtitles = false,
                Sound = "Bulgarian",
                Price = 2.40m,
                Format = "3D",
                Movie = movie
            };

            User user = new User()
            {
                FirstName = "Test",
                LastName = "User2",
                Email = "test2@abv.bg"
            };

            Ticket ticket = new Ticket()
            {
                Projection = projection,
                User = user,
                Seat = 5
            };

            await repository.AddAsync(genre);
            await repository.AddAsync(movie);
            await repository.AddAsync(projection);
            await repository.AddAsync(user);
            await repository.AddAsync(ticket);
            await repository.SaveChangesAsync();

            Projection projectionFromDb = repository.All<Projection>()
                .First();

            projectionId = projectionFromDb.Id;

            User userFromDb = repository.All<User>()
                .First();

            userId = userFromDb.Id;
        }
    }
}

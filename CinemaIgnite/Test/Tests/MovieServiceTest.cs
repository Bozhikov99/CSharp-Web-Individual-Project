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
using System.Threading.Tasks;

namespace Test.Tests
{
    public class MovieServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IMovieService service;
        private IMapper mapper;
        private string testGenreId;
        private string testMovieId;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddAutoMapper(cfg => cfg.AddProfile<MovieProfile>())
                .AddAutoMapper(cfg => cfg.AddProfile<UserProfile>())
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
            IRepository repository = serviceProvider.GetService<IRepository>();
            IEnumerable<Movie> all = repository.All<Movie>();

            int actual = all.Count();

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

            IRepository repository = serviceProvider.GetService<IRepository>();
            IEnumerable<Movie> all = repository.All<Movie>();

            Assert.AreEqual(expected, all.Count());
        }

        [Test]
        public async Task Delete_ThrowsException_WhenIdIsIncorrect()
        {
            string id = "random";

            Assert.ThrowsAsync<ArgumentException>(async () => await service.Delete(id));
        }

        [Test]
        [TestCase("TeSt")]
        [TestCase("GeNrE")]
        [TestCase("aUstria-")]
        public async Task Search_FindsMoviesCorrectly(string search)
        {
            int expected = 2;
            string lowerCaseSearch = search.ToLower();

            ListMovieModel[] movies = await service
                .GetAll(
                    m => m.Title.ToLower().Contains(lowerCaseSearch) ||
                    m.Genres.Select(g => g.Name).Any(g => g.ToLower().Contains(lowerCaseSearch)) ||
                    m.Country.ToLower().Contains(lowerCaseSearch))
                as ListMovieModel[];

            Assert.AreEqual(expected, movies.Length);
        }

        [Test]
        [TestCase("Arn")]
        [TestCase("Kimmich")]
        public async Task Search_FiltersCorrectly(string search)
        {
            int expected = 1;
            string lowerCaseSearch = search.ToLower();

            ListMovieModel[] movies = await service
                .GetAll(m => m.Actors.ToLower().Contains(lowerCaseSearch))
                as ListMovieModel[];

            Assert.AreEqual(expected, movies.Length);
        }

        [Test]
        public async Task GetEditModel_ReturnsCorrectly()
        {
            string title = "Test";
            string imageUrl = "SomeUrl";
            string description = "Simple description about a test movie";
            TimeSpan duration = new TimeSpan(1, 45, 2);
            int releaseYear = 1991;
            string actors = "Arnold Weissneger";

            string director = "Some Director";
            string country = "Austria-Hungary";

            EditMovieModel movie = await service.GetEditModel(testMovieId);

            Assert.AreEqual(title, movie.Title);
            Assert.AreEqual(imageUrl, movie.ImageUrl);
            Assert.AreEqual(description, movie.Description);
            Assert.AreEqual(duration, movie.Duration);
            Assert.AreEqual(releaseYear, movie.ReleaseYear);
            Assert.AreEqual(actors, movie.Actors);
            Assert.AreEqual(director, movie.Director);
            Assert.AreEqual(country, movie.Country);
        }

        [Test]
        public async Task Edit_Succesfully()
        {
            string newTitle = "Another Title";

            EditMovieModel movie = await service.GetEditModel(testMovieId);
            movie.GenreIds = new string[] { testGenreId };
            movie.Title = newTitle;

            await service.Edit(movie);

            EditMovieModel editedMovie = await service.GetEditModel(testMovieId);

            Assert.AreEqual(movie.Title, editedMovie.Title);
        }

        [Test]
        public async Task GetDetails_ReturnsCorrectly()
        {
            string title = "Test";
            string imageUrl = "SomeUrl";
            string description = "Simple description about a test movie";
            TimeSpan duration = new TimeSpan(1, 45, 2);
            int releaseYear = 1991;
            string actors = "Arnold Weissneger";
            string director = "Some Director";
            string country = "Austria-Hungary";

            MovieDetailsModel detailsModel = await service.GetMovieDetails(testMovieId);

            Assert.AreEqual(title, detailsModel.Title);
            Assert.AreEqual(imageUrl, detailsModel.ImageUrl);
            Assert.AreEqual(description, detailsModel.Description);
            Assert.AreEqual(duration, detailsModel.Duration);
            Assert.AreEqual(releaseYear, detailsModel.ReleaseYear);
            Assert.AreEqual(actors, detailsModel.Actors);
            Assert.AreEqual(director, detailsModel.Director);
            Assert.AreEqual(country, detailsModel.Country);
        }

        [Test]
        public async Task GetRating_ReturnsSuccessfully()
        {
            string expected = "4,0";

            string actual = await service.GetRating(testMovieId);

            Assert.AreEqual(expected, actual);
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

            User firstUser = new User()
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@abv.bg"
            };

            User secondUser = new User()
            {
                FirstName = "Test",
                LastName = "User2",
                Email = "test2@abv.bg"
            };

            await repository.AddAsync(testGenre);
            await repository.AddAsync(firstUser);
            await repository.AddAsync(secondUser);
            await repository.AddAsync(firstTestMovie);
            await repository.AddAsync(secondTestMovie);
            await repository.SaveChangesAsync();

            Movie movie = repository.All<Movie>().First(m => m.Title == "Test");
            testMovieId = movie.Id;

            Genre genre = repository.All<Genre>().First();
            testGenreId = genre.Id;

            User firstUserFromDb = repository.All<User>().First(u => u.Email == "test@abv.bg");
            User secondUserFromDb = repository.All<User>().First(u => u.Email == "test2@abv.bg");

            Rating firstRating = new Rating()
            {
                Value = 2,
                UserId = firstUserFromDb.Id,
                User = firstUserFromDb,
                Movie = movie
            };

            Rating secondRating = new Rating()
            {
                Value = 6,
                UserId = secondUserFromDb.Id,
                User = secondUserFromDb,
                Movie = movie
            };

            await repository.AddAsync(firstRating);
            await repository.AddAsync(secondRating);
            await repository.SaveChangesAsync();
        }
    }
}

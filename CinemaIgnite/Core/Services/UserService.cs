using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Services.Contracts;
using Core.ViewModels.Ticket;
using Core.ViewModels.User;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private const string logInErrorMessage = "Invalid data";
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRepository repository;

        public UserService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IRepository repository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.repository = repository;
        }

        public async Task<(bool isLoggedIn, string error)> Login(LoginUserModel model)
        {
            bool isLoggedIn = false;
            string error = string.Empty;
            string email = model.Email;

            User? user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                error = logInErrorMessage;
                return (isLoggedIn, error);
            }

            bool isValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isValidPassword)
            {
                error = logInErrorMessage;
                return (isLoggedIn, error);
            }

            await signInManager.SignInAsync(user, true);
            isLoggedIn = true;

            return (isLoggedIn, error);
        }

        public async Task<IdentityResult?> Register(RegisterUserModel model)
        {
            User user = mapper.Map<User>(model);
            user.UserName = model.Email;

            IdentityResult? result = await userManager.CreateAsync(user, model.Password);

            return result;
        }

        public async Task<bool> AddMovieToFavourites(string movieId)
        {
            bool isAdded = false;
            string userId = GetUserId();

            Movie movie = await repository.GetByIdAsync<Movie>(movieId);
            User user = await repository.GetByIdAsync<User>(userId);

            user.FavouriteMovies.Add(movie);

            try
            {
                repository.Update(user);
                await repository.SaveChangesAsync();
                isAdded = true;
            }
            catch (Exception)
            {
            }

            return isAdded;
        }

        public async Task<bool> RemoveMovieFromFavourites(string movieId)
        {
            bool isRemoved = false;
            string userId = GetUserId();

            Movie movie = await repository.GetByIdAsync<Movie>(movieId);
            User user = repository.All<User>()
                .Include(u => u.FavouriteMovies)
                .First(u => u.Id == userId);

            user.FavouriteMovies.Remove(movie);

            try
            {
                repository.Update(user);
                await repository.SaveChangesAsync();
                isRemoved = true;
            }
            catch (Exception)
            {
            }

            return isRemoved;
        }

        public async Task<UserProfileModel> GetUserProfile()
        {
            string userId = GetUserId();
            User user = await repository.GetByIdAsync<User>(userId);
            UserProfileModel model = mapper.Map<UserProfileModel>(user);

            return model;
        }

        public bool HasFavouriteMovie(string movieId)
        {
            string userId = GetUserId();
            var user = repository.All<User>()
                .Include(u => u.FavouriteMovies)
                .First(u => u.Id == userId);

            bool isFavourite = user.FavouriteMovies.Any(m => m.Id == movieId);

            return isFavourite;
        }

        public (bool hasRating, int? value) GetRating(string movieId)
        {
            int? value = 0;
            string userId = GetUserId();
            var user = repository.All<User>()
                .Include(u => u.Ratings)
                .First(u => u.Id == userId);

            bool hasRating = user.Ratings.Any(r => r.MovieId == movieId);

            if (hasRating)
            {
                Rating rating = repository.All<Rating>()
                    .First(r => r.MovieId == movieId && r.UserId == user.Id);

                value = rating.Value;
            }

            return (hasRating, value);
        }

        public async Task<bool> RateMovie(string movieId, int value)
        {
            bool isSuccessful = false;

            string userId = GetUserId();
            var user = repository.All<User>()
                .Include(u => u.Ratings)
                .First(u => u.Id == userId);

            Rating currentRating = user.Ratings
                .FirstOrDefault(r => r.MovieId == movieId);

            if (currentRating != null)
            {
                user.Ratings.Remove(currentRating);
            }


            Rating newRating = new Rating()
            {
                MovieId = movieId,
                UserId = userId,
                Value = value
            };

            try
            {
                user.Ratings.Add(newRating);
                repository.Update(user);
                await repository.SaveChangesAsync();
                isSuccessful = true;
            }
            catch (Exception)
            {

            }

            return isSuccessful;
        }

        public string GetUserId()
        {
            string userId = httpContextAccessor.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return userId;
        }

        public bool IsLoggedIn()
        {
            string userId = GetUserId();

            return userId != null;
        }

        public async Task<IEnumerable<ListTicketModel>> GetUpcomingTickets()
        {
            string userId = GetUserId();
            IEnumerable<ListTicketModel> upcomingTickets = await repository.All<Ticket>(t => t.Projection.Date > DateTime.Now)
                .ProjectTo<ListTicketModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return upcomingTickets;
        }

    }
}

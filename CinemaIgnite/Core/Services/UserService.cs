using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Core.ViewModels.User;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRepository repository;

        public UserService(
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IRepository repository,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.repository = repository;
            this.roleManager = roleManager;
        }

        public async Task Login(LoginUserModel model)
        {
            string email = model.Email;

            User? user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new ArgumentException(ErrorMessagesConstants.InvalidLoginData);
            }

            bool isValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isValidPassword)
            {
                throw new ArgumentException(ErrorMessagesConstants.InvalidLoginData);
            }

            await signInManager.SignInAsync(user, true);
        }

        public async Task<IdentityResult?> Register(RegisterUserModel model)
        {
            User user = mapper.Map<User>(model);
            user.UserName = model.Email;

            IdentityResult? result = await userManager.CreateAsync(user, model.Password);

            return result;
        }

        public async Task<EditUserModel> GetEditModel(string userId)
        {
            User user = await repository.GetByIdAsync<User>(userId);
            EditUserModel model = mapper.Map<EditUserModel>(user);

            return model;
        }

        public async Task Edit(EditUserModel model)
        {
            User user = await repository.GetByIdAsync<User>(model.Id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            repository.Update(user);
            await repository.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            await repository.DeleteAsync<User>(id);
            await repository.SaveChangesAsync();
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

        public async Task<IEnumerable<UserListModel>> GetUsers()
        {
            IEnumerable<UserListModel> users = await repository.All<User>()
                .ProjectTo<UserListModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return users;
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

        public async Task<IEnumerable<ListMovieModel>> GetFavouriteMovies()
        {
            string userId = GetUserId();
            User user = await repository.GetByIdAsync<User>(userId);

            IEnumerable<ListMovieModel> favouriteMovies = await repository.All<Movie>(m => m.UsersFavourited.Contains(user))
                .ProjectTo<ListMovieModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return favouriteMovies;
        }

        public async Task<bool> IsAdmin()
        {
            string userId = GetUserId();
            User user = await repository.GetByIdAsync<User>(userId);

            bool isAdmin = await userManager.IsInRoleAsync(user, RoleConstants.Administrator);

            return isAdmin;
        }

        public async Task<(UserRoleModel user, IEnumerable<SelectListItem> roles)> GetUserWithRoles(string id)
        {
            User user = await repository.GetByIdAsync<User>(id);
            UserRoleModel model = mapper.Map<UserRoleModel>(user);

            IEnumerable<SelectListItem> roles = roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = userManager.IsInRoleAsync(user, r.Name).Result
                })
                .ToArray();

            return (model, roles);
        }

        public async Task EditRoles(UserRoleModel model)
        {
            User user = await repository.GetByIdAsync<User>(model.Id);
            IEnumerable<string> userRoles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.RoleNames.Length > 0)
            {
                await userManager.AddToRolesAsync(user, model.RoleNames);
            }
        }
    }
}

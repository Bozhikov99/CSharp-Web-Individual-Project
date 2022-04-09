using Core.ViewModels.Movie;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListModel>> GetUsers();

        Task<IdentityResult?> Register(RegisterUserModel model);

        Task Login(LoginUserModel model);

        Task<EditUserModel> GetEditModel(string id);

        Task Edit(EditUserModel model);

        Task Delete(string id);

        Task<UserProfileModel> GetUserProfile();

        Task<bool> AddMovieToFavourites(string movieId);

        Task<bool> RemoveMovieFromFavourites(string movieId);

        Task<bool> RateMovie(string movieId, int value);

        Task<IEnumerable<ListMovieModel>> GetFavouriteMovies();

        Task<(UserRoleModel user, IEnumerable<SelectListItem> roles)> GetUserWithRoles(string id);

        Task EditRoles(UserRoleModel model);

        bool HasFavouriteMovie(string movieId);

        (bool hasRating, int? value) GetRating(string movieId);

        string GetUserId();

        Task<bool> IsAdmin();

    }
}

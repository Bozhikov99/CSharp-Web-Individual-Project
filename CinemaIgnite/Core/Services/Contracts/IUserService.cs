using Core.ViewModels.Movie;
using Core.ViewModels.Ticket;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListModel>> GetUsers();

        Task<IdentityResult?> Register(RegisterUserModel model);

        Task<(bool isLoggedIn, string error)> Login(LoginUserModel model);

        Task<EditUserModel> GetEditModel(string id);

        Task<bool> Edit(EditUserModel model);

        Task<bool> Delete(string id);

        Task<UserProfileModel> GetUserProfile();

        Task<bool> AddMovieToFavourites(string movieId);

        Task<bool> RemoveMovieFromFavourites(string movieId);

        Task<bool> RateMovie(string movieId, int value);

        Task<IEnumerable<ListTicketModel>> GetUpcomingTickets();

        Task<IEnumerable<ListMovieModel>> GetFavouriteMovies();

        Task<(UserRoleModel user, IEnumerable<SelectListItem> roles)> GetUserWithRoles(string id);

        Task<bool> EditRoles(UserRoleModel model);

        bool HasFavouriteMovie(string movieId);

        (bool hasRating, int? value) GetRating(string movieId);

        bool IsLoggedIn();

        string GetUserId();

        Task<bool> IsAdmin();

    }
}

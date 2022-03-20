using Core.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public interface IUserService
    {
        Task<IdentityResult?> Register(RegisterUserModel model);

        Task<(bool isLoggedIn, string error)> Login(LoginUserModel model);

        Task<UserProfileModel> GetUserProfile();

        Task<bool> AddMovieToFavourites(string movieId);

        Task<bool> RemoveMovieFromFavourites(string movieId);

        Task<bool> RateMovie(string movieId, int value);

        bool HasFavouriteMovie(string movieId);

        (bool hasRating, int? value) GetRating(string movieId);

        bool IsLoggedIn();

        string GetUserId();
    }
}

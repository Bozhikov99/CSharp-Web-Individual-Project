using AutoMapper;
using Core.Services.Contracts;
using Core.ViewModels.User;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        public UserService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
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

        public string GetUserId()
        {
            string userId = httpContextAccessor.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return userId;
        }
    }
}

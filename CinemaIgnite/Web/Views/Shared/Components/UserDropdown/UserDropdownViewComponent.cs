using Core.Services.Contracts;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.UserDropdown
{
    public class UserDropdownViewComponent: ViewComponent
    {
        private readonly IUserService userService;

        public UserDropdownViewComponent(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            UserProfileModel model = await userService.GetUserProfile();

            return View(model);
        }
    }
}

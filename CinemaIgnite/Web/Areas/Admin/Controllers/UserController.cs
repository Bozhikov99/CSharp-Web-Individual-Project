using Common;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            this.userService = userService;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> ManageUsers()
        {
            IEnumerable<UserListModel> users = await userService.GetUsers();

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditUserModel user = await userService.GetEditModel(id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserModel model)
        {
            if (!ModelState.IsValid)
            {
                EditUserModel user = await userService.GetEditModel(model.Id);
                return View(user);
            }

            bool isEdited = await userService.Edit(model);

            if (!isEdited)
            {

            }

            return RedirectToAction(nameof(ManageUsers));
        }

        public async Task<IActionResult> Delete(string id)
        {
            bool isDeleted = await userService.Delete(id);

            if (!isDeleted)
            {
                return View("UserError", ErrorMessagesConstants.ErrorDeletingUser);
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        //Uncomment to create a new role
        public async Task<IActionResult> CreateRole()
        {
            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Administrator"
            //});

            return Ok();
        }
    }
}

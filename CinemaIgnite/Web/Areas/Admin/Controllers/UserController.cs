using Common;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> ManageUsers(int activePage = 0)
        {
            IEnumerable<UserListModel> users = await userService.GetUsers();

            int pages = 0;

            if (users.Count() <= 10)
            {
                pages++;
            }
            else
            {
                pages = users.Count() / 10;

                if (users.Count() % 10 != 0)
                {
                    pages++;
                }
            }

            ViewBag.PagesCount = pages;
            ViewBag.PageLimit = 10;
            ViewBag.ActivePage = activePage;
            ViewBag.Controller = "User";
            ViewBag.Action = "ManageUsers";

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditUserModel user = await userService.GetEditModel(id);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public async Task<IActionResult> Roles(string id)
        {
            (UserRoleModel user, IEnumerable<SelectListItem> roles) = await userService.GetUserWithRoles(id);
            ViewBag.Roles = roles;

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Roles(UserRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                (UserRoleModel userModel, IEnumerable<SelectListItem> roles) = await userService.GetUserWithRoles(model.Id);
                ViewBag.Roles = roles;

                return View(userModel);
            }

            bool isEdited = await userService.EditRoles(model);

            if (!isEdited)
            {
                return View("UserError", "X");
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

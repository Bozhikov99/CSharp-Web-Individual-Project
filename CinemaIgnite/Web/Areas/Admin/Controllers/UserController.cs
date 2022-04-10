using Common;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IHtmlLocalizer<UserController> localizer;

        public UserController(RoleManager<IdentityRole> roleManager, IUserService userService, IHtmlLocalizer<UserController> localizer)
        {
            this.userService = userService;
            this.localizer = localizer;
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

            var names = localizer["Names"];
            var email = localizer["Email"];
            var options = localizer["Options"];
            var editButton = localizer["EditButton"];
            var deleteButton = localizer["DeleteButton"];
            var roleButton = localizer["RoleButton"];

            ViewData["Names"] = names;
            ViewData["Email"] = email;
            ViewData["Options"] = options;
            ViewData["EditButton"] = editButton;
            ViewData["DeleteButton"] = deleteButton;
            ViewData["RoleButton"] = roleButton;

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditUserModel user = await userService.GetEditModel(id);

            var editTitle = localizer["EditButton"];
            var editHeadline = localizer["EditHeadline"];
            var namePlaceholder = localizer["NamePlaceholder"];
            var familyPlaceholder = localizer["FamilyPlaceholder"];

            ViewData["EditPageTitle"] = editTitle;
            ViewData["EditHeadline"] = editHeadline;
            ViewData["NamePlaceholder"] = namePlaceholder;
            ViewData["FamilyPlaceholder"] = familyPlaceholder;

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

            try
            {
                await userService.Edit(model);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorEditingUser);
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await userService.Delete(id);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorDeletingUser);
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        public async Task<IActionResult> Roles(string id)
        {
            (UserRoleModel user, IEnumerable<SelectListItem> roles) = await userService.GetUserWithRoles(id);
            ViewBag.Roles = roles;

            var roleUser = localizer["RoleUser"];
            var submit = localizer["Submit"];
            var rolesPageTitle = localizer["RolesPageTitle"];
            var localizerRoles = localizer["Roles"];

            ViewData["LocalizedRoles"] = localizerRoles;
            ViewData["RolesPageTitle"] = rolesPageTitle;
            ViewData["Submit"] = submit;
            ViewData["RoleUser"] = roleUser;

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

            try
            {
                await userService.EditRoles(model);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorEditingUserRoles);
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

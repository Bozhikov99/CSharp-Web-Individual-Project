using Core.Services.Contracts;
using Core.ViewModels.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Web.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        private readonly INotificationService notificationService;
        private readonly IUserService userService;
        private readonly IHtmlLocalizer<NotificationController> localizer;

        public NotificationController(INotificationService notificationService, IUserService userService, IHtmlLocalizer<NotificationController> localizer)
        {
            this.notificationService = notificationService;
            this.userService = userService;
            this.localizer = localizer;
        }

        [HttpPost]
        public async Task<IActionResult> Read(string id)
        {
            await notificationService.Read(id);
            string userId = userService.GetUserId();
            int unreadCount = await notificationService.GetUnreadCount(userId);
            return Ok(unreadCount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string[] ids)
        {
            await notificationService.Delete(ids);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All(int activePage = 0)
        {
            string userId = userService.GetUserId();
            IEnumerable<NotificationDetailsModel> notifications = await notificationService
                .GetAll(userId);

            int pages = 0;

            if (notifications.Count() <= 5)
            {
                pages++;
            }
            else
            {
                pages = notifications.Count() / 5;

                if (notifications.Count() % 5 != 0)
                {
                    pages++;
                }
            }

            //Pagination parameters
            ViewBag.PagesCount = pages;
            ViewBag.PageLimit = 5;
            ViewBag.ActivePage = activePage;
            ViewBag.Controller = "Notification";
            ViewBag.Action = "All";

            //Localization parameters
            var notificationsHeadline = localizer["NotificationsHeadline"];
            var noResults = localizer["NoResults"];

            ViewData["NotificationsHeadline"] = notificationsHeadline;
            ViewData["NoResults"] = noResults;

            return View(notifications);
        }
    }
}

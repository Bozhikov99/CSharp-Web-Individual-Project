using Core.Services.Contracts;
using Core.ViewModels.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService notificationService;
        private readonly IUserService userService;

        public NotificationController(INotificationService notificationService, IUserService userService)
        {
            this.notificationService = notificationService;
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Read(string id)
        {
            await notificationService.Read(id);
            string userId = userService.GetUserId();
            ViewBag.Unread = notificationService.GetUnreadCount(userId);
            return new EmptyResult();
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

            ViewBag.PagesCount = pages;
            ViewBag.PageLimit = 5;
            ViewBag.ActivePage = activePage;
            ViewBag.Controller = "Notification";
            ViewBag.Action = "All";

            return View(notifications);
        }
    }
}

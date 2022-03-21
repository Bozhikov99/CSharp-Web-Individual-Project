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

        public async Task<IActionResult> Read(string id)
        {
            await notificationService.Read(id);

            return new EmptyResult();
        }

        public async Task<IActionResult> Delete(string id)
        {
            await notificationService.Delete(id);

            return new EmptyResult();
        }

        public async Task<IActionResult> All()
        {
            string userId = userService.GetUserId();
            IEnumerable<NotificationDetailsModel> notifications = await notificationService
                .GetAll(userId);

            return View(notifications);
        }
    }
}

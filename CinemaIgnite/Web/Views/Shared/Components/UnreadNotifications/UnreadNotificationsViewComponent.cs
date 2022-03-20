using Core.Services.Contracts;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.UnreadNotifications
{
    public class UnreadNotificationsViewComponent : ViewComponent
    {
        private readonly INotificationService notificationService;
        private readonly IUserService userService;

        public UnreadNotificationsViewComponent(INotificationService notificationService, IUserService userService)
        {
            this.notificationService = notificationService;
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = userService.GetUserId();
            int unread = await notificationService.GetUnreadCount(userId);
            ViewBag.UserId = userId;

            return View(unread);
        }
    }
}

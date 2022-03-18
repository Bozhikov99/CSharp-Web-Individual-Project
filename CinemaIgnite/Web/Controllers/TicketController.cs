using Core.Services.Contracts;
using Core.ViewModels.Ticket;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class TicketController : BaseController
    {
        private readonly ITicketService ticketService;
        private readonly IUserService userService;

        public TicketController(ITicketService ticketService, IUserService userService)
        {
            this.ticketService = ticketService;
            this.userService = userService;
        }

        public IActionResult Create(string id)
        {
            string userId = userService.GetUserId();
            ViewBag.UserId = userId;
            ViewBag.ProjectionId = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateForm")]
        public async Task<IActionResult> Create(CreateTicketModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create));
            }

            string userId = userService.GetUserId();

            (bool isCreated, string error) = await ticketService.Create(model, userId);

            if (!isCreated)
            {
                return View("UserError", error);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

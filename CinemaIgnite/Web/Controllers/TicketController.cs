using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Core.ViewModels.Projection;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Web.Controllers
{
    [Authorize]
    public class TicketController : BaseController
    {
        private readonly ITicketService ticketService;
        private readonly IUserService userService;
        private readonly IHtmlLocalizer<TicketController> localizer;

        public TicketController(ITicketService ticketService, IUserService userService, IHtmlLocalizer<TicketController> localizer)
        {
            this.ticketService = ticketService;
            this.userService = userService;
            this.localizer = localizer;
        }

        public async Task<IActionResult> Create(string id)
        {
            string userId = userService.GetUserId();
            IEnumerable<int> seatsTaken = await ticketService.GetTakenSeats(id);

            (ListMovieModel movieModel, ListProjectionModel projectionModel) = await ticketService.GetInfo(id);

            ViewBag.UserId = userId;
            ViewBag.ProjectionId = id;
            ViewBag.SeatsTaken = seatsTaken;
            ViewBag.Movie = movieModel;
            ViewBag.Projection = projectionModel;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int[] seats, string projectionId, string userId)
        {

            try
            {
                await ticketService.BuyTickets(seats, projectionId, userId);
            }
            catch (ArgumentException ae)
            {
                return View("UserError", ae.Message);
            }
            catch (Exception)
            {
                return View("UserError", ErrorMessagesConstants.ErrorBuyingTicket);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

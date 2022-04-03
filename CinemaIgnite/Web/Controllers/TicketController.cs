using Core.Services.Contracts;
using Core.ViewModels.Movie;
using Core.ViewModels.Projection;
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

            (bool isSuccesful, string error) = await ticketService.BuyTickets(seats, projectionId, userId);

            if (!isSuccesful)
            {
                return View("UserError", error);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

using Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService ticketService;

        public TicketController(ITicketService ticketService)
        {
            this.ticketService = ticketService;
        }

        //public Task<IActionResult> Create()
        //{
        //    return View();
        //}
    }
}

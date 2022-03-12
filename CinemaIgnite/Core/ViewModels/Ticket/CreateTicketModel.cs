using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Ticket
{
    public class CreateTicketModel
    {
        [Range(TicketConstants.SeatMin, TicketConstants.SeatMax)]
        public int Seat { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string ProjectionId { get; set; }
    }
}

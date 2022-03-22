using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Ticket
{
    public class ListTicketModel
    {
        public string MovieTitle { get; set; }

        public DateTime Date { get; set; }

        public int Seat { get; set; }
    }
}

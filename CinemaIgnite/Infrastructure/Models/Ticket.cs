using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Ticket
    {
        public Ticket()
        {
            Id = Guid.NewGuid()
                .ToString();
        }

        [Key]
        public string Id { get; set; }

        [Range(TicketConstants.SeatMin, TicketConstants.SeatMax)]
        public int Seat { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey(nameof(Projection))]
        public string ProjectionId { get; set; }

        public virtual Projection Projection { get; set; }
    }
}

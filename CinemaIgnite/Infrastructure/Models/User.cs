using Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Id = Guid.NewGuid()
                .ToString();

            Tickets = new List<Ticket>();

            FavouriteMovies = new List<Movie>();

            Notifications = new List<Notification>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(UserConstants.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserConstants.NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(UserConstants.EmailMaxLength)]
        public string Email { get; set; }


        public virtual ICollection<Ticket> Tickets { get; set; }

        public virtual ICollection<Movie> FavouriteMovies { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
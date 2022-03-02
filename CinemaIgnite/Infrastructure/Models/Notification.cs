using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Notification
    {
        public Notification()
        {
            Id = Guid.NewGuid()
                .ToString();

            IsChecked = false;
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(NotificationConstants.TextMaxLength)]
        public string Text { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsChecked { get; set; }
    }
}

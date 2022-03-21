using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Notification
{
    public class NotificationDetailsModel
    {
        [UIHint("Hidden")]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public bool IsChecked { get; set; }
    }
}
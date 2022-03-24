using Common;
using Common.ValidationConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.User
{
    public class EditUserModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(UserConstants.NameMaxLength, MinimumLength = UserConstants.NameMinLength, ErrorMessage = ErrorMessagesConstants.InvalidLengthMessage)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(UserConstants.NameMaxLength, MinimumLength = UserConstants.NameMinLength, ErrorMessage = ErrorMessagesConstants.InvalidLengthMessage)]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}

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
    public class LoginUserModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(UserConstants.EmailMaxLength, MinimumLength = UserConstants.EmailMinLength, ErrorMessage = ErrorMessagesConstants.InvalidLengthMessage)]
        public string Email { get; set; }
    }
}

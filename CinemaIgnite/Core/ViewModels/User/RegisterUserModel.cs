using Common;
using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.User
{
    public class RegisterUserModel
    {

        [Required]
        [StringLength(UserConstants.NameMaxLength, MinimumLength = UserConstants.NameMinLength, ErrorMessage = ErrorMessagesConstants.InvalidLengthMessage)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(UserConstants.NameMaxLength, MinimumLength = UserConstants.NameMinLength, ErrorMessage = ErrorMessagesConstants.InvalidLengthMessage)]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = ErrorMessagesConstants.PasswordsNotMatchingMessage)]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(UserConstants.EmailMaxLength, MinimumLength = UserConstants.EmailMinLength, ErrorMessage = ErrorMessagesConstants.InvalidLengthMessage)]
        public string Email { get; set; }
    }
}

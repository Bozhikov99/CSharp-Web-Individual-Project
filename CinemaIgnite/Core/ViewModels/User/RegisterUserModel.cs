using Common;
using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.User
{
    public class RegisterUserModel
    {

        [Required]
        [StringLength(UserConstants.NameMaxLength, MinimumLength = UserConstants.NameMinLength, ErrorMessage = ErrorMessagesConstants.InvalidLengthMessage)]
        [RegularExpression(UserConstants.NameRegex, ErrorMessage = ErrorMessagesConstants.InvalidUserName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(UserConstants.NameMaxLength, MinimumLength = UserConstants.NameMinLength, ErrorMessage = ErrorMessagesConstants.InvalidLengthMessage)]
        [RegularExpression(UserConstants.NameRegex, ErrorMessage = ErrorMessagesConstants.InvalidUserName)]
        public string LastName { get; set; }

        [Required]
        [StringLength(UserConstants.PasswordMaxLength, MinimumLength = UserConstants.PasswordMinLength, ErrorMessage = ErrorMessagesConstants.InvalidLengthMessage)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = ErrorMessagesConstants.PasswordsNotMatchingMessage)]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(UserConstants.EmailMaxLength, MinimumLength = UserConstants.EmailMinLength, ErrorMessage = ErrorMessagesConstants.InvalidLengthMessage)]
        public string Email { get; set; }
    }
}

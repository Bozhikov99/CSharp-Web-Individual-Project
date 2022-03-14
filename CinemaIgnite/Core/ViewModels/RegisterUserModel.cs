using Common;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels
{
    public class RegisterUserModel
    {
        [Required]
        [MaxLength(UserConstants.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserConstants.NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(UserConstants.EmailMaxLength)]
        public string Email { get; set; }
    }
}

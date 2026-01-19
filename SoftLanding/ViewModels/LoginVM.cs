using System.ComponentModel.DataAnnotations;

namespace SoftLanding.ViewModels.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email or Username is required.")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPersisted { get; set; } 
    }
}
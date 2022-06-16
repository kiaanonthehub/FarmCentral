using System.ComponentModel.DataAnnotations;

namespace FarmCentralWebApp.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "A valid email address is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "A valid password for this account is required to log in")]
        public string Password { get; set; }
        [Required(ErrorMessage = "A valid role is required")]
        public string Role { get; set; }
    }
}

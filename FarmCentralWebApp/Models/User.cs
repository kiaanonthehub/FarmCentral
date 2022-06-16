using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FarmCentralWebApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required (ErrorMessage = "Enter in your firstname")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Enter in your lastname")]
        public string Surname { get; set; }
       
        [Required]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
       
        [Required(ErrorMessage = "A valid role of 'Farmer' must be selected")]
        public string Role { get; set; }
    }
}

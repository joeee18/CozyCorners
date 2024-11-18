using System.ComponentModel.DataAnnotations;

namespace CozyCorners.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Username must contain only letters and numbers with no spaces.")]
        public string UserName { get; set; }
        
        [EmailAddress(ErrorMessage = "Stucture Of Email Is not Valid")]
        public string Email { get; set; }

        

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$", ErrorMessage = "The password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }

   

    }
}

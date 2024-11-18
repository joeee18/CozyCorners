using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace CozyCorners.ViewModels
{
    public class UserFormViewModel
    {

        [Required]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Username must contain only letters and numbers with no spaces.")]
        public string UserName { get; set; }

        [Length(4,50,ErrorMessage ="MinLength is 4 ,Max Length is 50")]
        public string? DisplayName { get; set; }

        [EmailAddress(ErrorMessage ="Stucture Of Email Is not Valid")]
        public string Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

       
        public IFormFile? PhotoFile { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$", ErrorMessage = "The password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string? Password { get; set; }

        [Display(Name ="Role")]
        public IEnumerable<SelectListItem> Roles { get; set; }=Enumerable.Empty<SelectListItem>();
        public string? RoleId { get; set; }
      

    }
}

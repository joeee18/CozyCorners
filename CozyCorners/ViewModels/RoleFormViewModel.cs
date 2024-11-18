using System.ComponentModel.DataAnnotations;

namespace CozyCorners.ViewModels
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage ="Name Is Required")]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}

using CozyCorners.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace CozyCorners.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        [MaxLength(50,ErrorMessage ="Max Length is 50")]
        public string Name { get; set; }
        public string? photo { get; set; }

        public string Description { get; set; }
        public IFormFile? FilePath { get; set; }

        //public int? ParentCategoryId { get; set; }
        //public Category ParentCategory { get; set; }
        //public ICollection<Category> SubCategories { get; set; }

    }
}

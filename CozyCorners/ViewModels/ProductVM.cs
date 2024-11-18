using CozyCorners.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CozyCorners.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; }
        public IFormFile? FilePath { get; set; }
        public string? PhotoPath { get; set; }
        public IReadOnlyList<SelectListItem> Categories { get; set; } =new List<SelectListItem>();

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public Category? Category  { get; set; }

    }
}

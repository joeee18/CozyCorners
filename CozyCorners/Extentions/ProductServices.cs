using CozyCorners.Core;
using CozyCorners.Core.Models;
using CozyCorners.Core.Models.Identity;
using CozyCorners.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CozyCorners.Extentions
{
    public class ProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductServices(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public  async Task<IReadOnlyList<SelectListItem>> GetAllCategories()
        {
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            return  categories.Select(r => new SelectListItem()
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();
        }
    
        public async Task<string> GetPhotoPath(ProductVM productVM)
        {
            var PhotoName = $"{Guid.NewGuid()}{Path.GetExtension(productVM.FilePath.FileName)}";
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "Products");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            var pathPhoto = Path.Combine(uploadDir, PhotoName);
            using (var fileStream = new FileStream(pathPhoto, FileMode.Create))
            {
                await productVM.FilePath.CopyToAsync(fileStream);
            }

            return $"/assets/images/Products/{PhotoName}";
        }


    }
}

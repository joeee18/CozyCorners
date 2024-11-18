using CozyCorners.Core.Models.Identity;
using CozyCorners.ViewModels;

namespace CozyCorners.Extentions
{
    public class CategoryServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryServices(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public  async Task<string> GetPhotoPath(CategoryVM categoryVM)
        {
            var PhotoName = $"{Guid.NewGuid()}{Path.GetExtension(categoryVM.FilePath.FileName)}";
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "Categories");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            var pathPhoto = Path.Combine(uploadDir, PhotoName);
            using (var fileStream = new FileStream(pathPhoto, FileMode.Create))
            {
                await categoryVM.FilePath.CopyToAsync(fileStream);
            }

            return $"/assets/images/Categories/{PhotoName}";
        }

    }
}

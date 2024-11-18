using CozyCorners.Core.Models.Identity;
using CozyCorners.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace CozyCorners.Extentions
{
    public static class UserServices
    {

        public static async Task<IEnumerable<SelectListItem>> GetAllRoles(this RoleManager<IdentityRole> roleManager)
        {
            return await roleManager.Roles.Select(r => new SelectListItem()
            {
                Value = r.Id.ToString(),
                Text = r.Name
            })
            .AsNoTracking()
            .ToListAsync();
        }
        public static async Task<string> GetPhotoPath(this UserManager<AppUser> userManager,UserFormViewModel addUser,IWebHostEnvironment hostEnvironment)
        {
            var PhotoName = $"{Guid.NewGuid()}{Path.GetExtension(addUser.PhotoFile.FileName)}";
            string uploadDir = Path.Combine(hostEnvironment.WebRootPath, "assets", "images", "Users");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            var pathPhoto = Path.Combine(uploadDir, PhotoName);
            using (var fileStream = new FileStream(pathPhoto, FileMode.Create))
            {
                await addUser.PhotoFile.CopyToAsync(fileStream);
            }

            return $"/assets/images/Users/{PhotoName}";
        }


    }
}

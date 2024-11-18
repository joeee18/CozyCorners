
using CozyCorners.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace CozyCorners.Controllers
{
	//[Authorize(AuthenticationSchemes = "Cookies")]
	[Authorize(Roles = "Admin")]
	public class RoleController : Controller
	{
		private readonly RoleManager<IdentityRole> roleManager;
  

        public RoleController(RoleManager<IdentityRole> roleManager)
		{
			this.roleManager = roleManager;
          
        }
		public async Task<IActionResult> Index()
		{
			var Roles = await roleManager.Roles.ToListAsync();
			return View(Roles);
		}


        // GET: RoleController/Create
        
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleFormViewModel roleViewModel)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            if (ModelState.IsValid)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleViewModel.Name);
                try
                {
                    if (!roleExist)
                    {
                        
                        await roleManager.CreateAsync(new IdentityRole(roleViewModel.Name.Trim()));
                        return RedirectToAction(nameof(Index));

                    }
                    else
                    {
                        ModelState.AddModelError("Name", "Role Name Is Exist");
                        return View("Index" ,await roleManager.Roles.ToListAsync());
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            await roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var mappedRole = new RoleViewModel()
            {
                RoleName = role.Name
            };
            return View(mappedRole);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RoleViewModel roleView)
        {
            if (ModelState.IsValid)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleView.RoleName);
                try
                {
                    if (!roleExist)
                    {

                        var role = await roleManager.FindByIdAsync(roleView.Id);
                        role.Name = roleView.RoleName;
                        await roleManager.UpdateAsync(role);
                        return RedirectToAction(nameof(Index));

                    }
                    else
                    {
                        ModelState.AddModelError("Name", "Role Name Is Exist");
                        return View("Index", await roleManager.Roles.ToListAsync());
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return RedirectToAction("Index");

        }

    }

}

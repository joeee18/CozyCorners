using Microsoft.AspNetCore.Mvc;

namespace CozyCorners.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		//public IActionResult IndexAdmin()
		//{
		//	return View();
		//}
		//public IActionResult DashBoard()
		//{
		//	ViewBag.Layout = "~/Views/Shared/_LayoutDashBoard.cshtml";


		//	return RedirectToAction("IndexAdmin", "Admin");
		//}
	}
}

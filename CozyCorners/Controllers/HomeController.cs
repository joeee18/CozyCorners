using CozyCorners.Core;
using CozyCorners.Core.Models;
using CozyCorners.Core.Repositories.Contract;
using CozyCorners.Models;
using CozyCorners.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace CozyCorners.Controllers
{
   //[Authorize(AuthenticationSchemes = "Cookies")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IProductRepository _product;

		public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork,IProductRepository product)
        {
            _logger = logger;
			_unitOfWork = unitOfWork;
			_product = product;
		}

        public async Task<IActionResult> Index(string searchString)
        {
			var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
			var topCategories = categories.Take(3).ToList();
			var products = await _product.GetAllAsync();

			if (!String.IsNullOrEmpty(searchString))
			{
				products = products.Where(n => n.Name.Contains(searchString)).ToList();
			}
			// Create ViewModel and populate data
			var model = new HomeViewModel
			{
				Categories = topCategories,
				Products = products
			};

			return View(model);

		}
	
		public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(SendMailDto sendMailDto)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                MailMessage mail = new MailMessage();
                // enter your email address
                mail.From = new MailAddress("mmaddminmm22@gmail.com");
                // To email address - you need to enter your to email address
                mail.To.Add("mazenmohsen11111@gmail.com");

                mail.Subject = sendMailDto.Subject;
                // you can specify also CC and BCC
                //mail.CC.Add("");
                //mail.Bcc.Add("");

                mail.IsBodyHtml = true;
                string content = "Name : " + sendMailDto.Name;
                content += "<br/> Message :" + sendMailDto.Message;

                mail.Body = content;

                //creat SMTP instant
                // you need to pass mail server address and you can also specify the port if you required
                SmtpClient smtpClient = new SmtpClient("mail.gmail.com");

                // create network credential and you need to give from email address and password
                NetworkCredential networkCredential = new NetworkCredential("mmaddminmm22@gmail.com", "admin22@AA");
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredential;
                smtpClient.Port = 25;
                smtpClient.EnableSsl = false;
                smtpClient.Send(mail);

                ViewBag.Message = "Mail Send";

                // need to create the from
                ModelState.Clear();

            }
            catch (Exception ex)
            {
                // if any error occured it will show
                ViewBag.Message = ex.Message.ToString();
            }

            return View();
        }

        public IActionResult Services()
        {
            return View();
        }
        public async Task<ActionResult<List<Category>>> About()
        {
            var categories =(List<Category>) await _unitOfWork.Repository<Category>().GetAllAsync();
            return View(categories);
        }
        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

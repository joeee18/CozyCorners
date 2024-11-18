using AutoMapper;
using CozyCorners.Core;
using CozyCorners.Core.Models;
using CozyCorners.Core.Repositories.Contract;
using CozyCorners.Extentions;
using CozyCorners.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CozyCorners.Controllers
{
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ProductServices _productServices;
        

        public ProductController(IUnitOfWork unitOfWork,IProductRepository productRepository, IMapper mapper, ProductServices productServices)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _mapper = mapper;
            _productServices = productServices;
            
        }
     
        public async Task<ActionResult> Index()
        {
            var Products = await _productRepository.GetAllAsync();

            var mappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(Products);
            return View(mappedProducts);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var product = await _productRepository.GetById(id);
            var ProductMapped = new ProductVM()
            {
                Name = product.Name,
                Description = product.Description,
                PhotoPath = product.PhotoPath,
                Price = product.Price,
                //CategoryId = product.CategoryId,
                Rating = product.Rating,
                CategoryName=product.Category.Name,

            };
            //var productMapped = _mapper.Map<Product, ProductVM>(product);
            return View(ProductMapped);
        }


        // GET: ProductController/Create
        public async Task<ActionResult> Create()
        {
            ProductVM productVM = new ProductVM();
            var categories = await _productServices.GetAllCategories();
            productVM.Categories=categories;
            return View(productVM);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var PhotoPath = await _productServices.GetPhotoPath(productVM);
                    productVM.PhotoPath = PhotoPath;
                  
                    //var ProductMapped = _mapper.Map<ProductVM, Product>(productVM);
                    var ProductMapped = new Product()
                    {
                        Name = productVM.Name,
                        Description =productVM.Description,
                        PhotoPath =productVM.PhotoPath,
                        Price=productVM.Price,
                        CategoryId=productVM.CategoryId,
                        Rating=productVM.Rating,
                    };
                    await _productRepository.Add(ProductMapped);
                    var RowsAdded = await _unitOfWork.Complet();
                    if (RowsAdded != null)
                    {
                        TempData["Message"] = $"{productVM.Name} Product Added Successfully!";
                        //ViewBag.Message =$"{CategoryMapped.Name} Category Added Successfully!";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    throw;
                }


            }
           
            var categories = await _productServices.GetAllCategories();
            productVM.Categories = categories;
            return View(productVM);
        }

       
        public async Task<ActionResult> Edit(int id)
        {
            var product = await _productRepository.GetById(id);
            var productMapped = _mapper.Map<Product, ProductVM>(product);
          
            var categories = await _productServices.GetAllCategories();
            productMapped.Categories = categories;
            return View(productMapped);
           
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (productVM.FilePath != null)
                    {
                        var PhotoPath = await _productServices.GetPhotoPath(productVM);
                        productVM.PhotoPath = PhotoPath;

                    }

                    var productMapped = new Product()
                    {
                        Name = productVM.Name,
                        Description = productVM.Description,
                        PhotoPath = productVM.PhotoPath ,
                        Price = productVM.Price,
                        CategoryId = productVM.CategoryId,
                        Rating = productVM.Rating,
                    };
                    _productRepository.update(productMapped);
                    var RowsAdded = await _unitOfWork.Complet();
                    if (RowsAdded != null)
                    {
                        TempData["Message"] = $"Updated Product Done Successfully!";
                        //ViewBag.Message =$"{CategoryMapped.Name} Category Added Successfully!";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    throw;
                }


            }
            return View(productVM);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {


                var product = await _productRepository.GetById(id);
               _productRepository.delete(product);
                var RowsAdded = await _unitOfWork.Complet();
                if (RowsAdded != null)
                {
                    TempData["Message"] = $"Product Deleted Successfully!";
                    //ViewBag.Message =$"{CategoryMapped.Name} Category Added Successfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                throw;
            }




        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            var product=await _productRepository.GetById(id);
            
            return View(product);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> SeeMore(int id)
        {
            var Products = await _productRepository.GetAllAsync();
            var products = Products.Where(p => p.CategoryId == id).ToList();


            var mappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(products);
            return View(mappedProducts);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Shop()
        {
            var Products = await _productRepository.GetAllAsync();
            var products = Products.Take(30).ToList();

            var mappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(products);
            return View(mappedProducts);
        }
    }
}
